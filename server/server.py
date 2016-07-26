#!/usr/bin/env python3.5
# -*- coding: utf-8 -*-

import sys
import os
import json
import tornado.web
import tornado.httpserver
import tornado.ioloop
import tornado.options
import tornado.websocket
import random
import string
from   configs.config   import configs

channel_auth = {}
senders = {}
recivers = {}
selector = {'s':senders,'r':recivers}
invertor = {'r':senders,'s':recivers}

msg = {
    'online': '{"action":"system","subaction":"peerstate","data":{"state":true}}',
    'offline': '{"action":"system","subaction":"peerstate","data":{"state":false}}'
}

if configs.localmode:
    channel_auth = {'local':'local'}


letters = string.digits + string.ascii_letters

def new_channel():
    channel_id = ''.join([random.choice(letters) for x in range(6)])
    auth_code = ''.join([random.choice(letters) for x in range(6)])
    if channel_id in channel_auth.keys():
        return gen_channel()
    channel_auth[channel_id] = auth_code
    return channel_id, auth_code

class BasicHandler(tornado.web.RequestHandler):
    def auth(self):
        if configs.localmode:
            return True
        self.channel_id = self.get_argument('c','local')
        self.auth = self.get_argument('a','local')
        if not self.channel_id in channel_auth.keys() or  channel_auth[self.channel_id] != self.auth:
            self.redirect('/auth_failed')
            return False
        return True

class index_handler(BasicHandler):
    def get(self):
        self.render('index.html',localmode=configs.localmode)

class new_handler(BasicHandler):
    def get(self):
        new_type = self.get_argument('type',None)
        if not new_type:
            self.redirect('/')
            return
        channel_id, auth_code = new_channel()
        self.redirect('/{}?c={}&a={}'.format(new_type, channel_id, auth_code))


class sender_handler(BasicHandler):
    def get(self):
        if not self.auth(): return
        self.render('sender.html')

class receiver_handler(BasicHandler):
    def get(self):
        if not self.auth(): return
        self.render('receiver.html')

class auth_failed_handler(BasicHandler):
    def get(self):
        self.render('auth_failed.html')

class localmode_handler(BasicHandler):
    def get(self):
        self.write(str(configs.localmode))

class ws_handler(tornado.websocket.WebSocketHandler):
    def open(self,_type):
        self.channel_id = self.get_argument('c','local')
        self.auth = self.get_argument('a','local')
        self.type = _type

        # Connection Verify
        if not configs.localmode:
            if not self.channel_id in channel_auth.keys():
                self.try_close('channel_not_exist')
                return
            if channel_auth[self.channel_id] != self.auth:
                self.try_close('auth_failed')
                return
        if self.channel_id in selector[self.type].keys():
            self.try_close('already_connected')
            return

        selector[self.type][self.channel_id] = self
        if self.channel_id in invertor[self.type].keys():
            invertor[self.type][self.channel_id].write_message(msg['online'])
            self.write_message(msg['online'])

    def on_close(self,*args,**kwargs):
        del(selector[self.type][self.channel_id])
        invertor[self.type][self.channel_id].write_message(msg['offline'])

    def on_message(self, message):
        if self.channel_id in invertor[self.type].keys():
            invertor[self.type][self.channel_id].write_message(message)

    def try_close(self,reason=''):
        if self.ws_connection is not None:
            self.close(reason=reason)

    def send_message(self,_dict):
        if self.ws_connection is not None:
            self.write_message(json.dumps(_dict))

    def check_origin(self, origin):
        # TODO: check orgin
        return True

def run():
    if not os.path.exists('logs'):
        os.makedirs('logs')
    args = sys.argv
    args.append("--log_file_prefix=logs/web.log")
    tornado.options.parse_command_line()
    app = tornado.web.Application(
        handlers=[
            (r'/?',index_handler),
            (r'/ws/(r)',ws_handler),
            (r'/ws/(s)',ws_handler),
            (r'/sender',sender_handler),
            (r'/receiver',receiver_handler),
            (r'/s',sender_handler),
            (r'/r',receiver_handler),
            (r'/new',new_handler),
            (r'/auth_failed',auth_failed_handler),
            (r'/localmode',localmode_handler),
        ],
        template_path='template',
        static_path='static',
        debug=configs.debug
    )
    http_server = tornado.httpserver.HTTPServer(app)
    http_server.listen(configs.port)
    tornado.ioloop.IOLoop.instance().start()

if __name__ == '__main__':
    run()
