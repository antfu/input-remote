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

channel_auth = {'test':'test'}
senders = {}
recivers = {}
selector = {'sender':senders,'receiver':recivers}
invertor = {'receiver':senders,'sender':recivers}

msg = {
    'online': '{"action":"system","subaction":"peerstate","data":{"state":true}}',
    'offline': '{"action":"system","subaction":"peerstate","data":{"state":false}}'
}

letters = string.digits + string.ascii_letters
def new_channel():
    channel_id = ''.join([random.choice(letters) for x in range(6)])
    auth_code = ''.join([random.choice(letters) for x in range(6)])
    if channel_id in channel_auth.keys():
        return gen_channel()
    channel_auth[channel_id] = auth_code
    return channel_id, auth_code


class index_handler(tornado.web.RequestHandler):
    def get(self):
        self.render('index.html')

class new_handler(tornado.web.RequestHandler):
    def get(self):
        new_type = self.get_argument('type',None)
        if not new_type:
            self.redirect('/')
            return
        channel_id, auth_code = new_channel()
        self.redirect('/{}?c={}&a={}'.format(new_type, channel_id, auth_code))


class sender_handler(tornado.web.RequestHandler):
    def get(self):
        self.render('sender.html')

class receiver_handler(tornado.web.RequestHandler):
    def get(self):
        self.render('receiver.html')

class ws_handler(tornado.websocket.WebSocketHandler):
    def open(self):
        self.channel_id = self.get_argument('c','default')
        self.auth = self.get_argument('a','default')
        self.type = self.get_argument('t','receiver')

        # Connection Verify
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
            (r'/ws',ws_handler),
            (r'/sender',sender_handler),
            (r'/receiver',receiver_handler),
            (r'/new',new_handler),
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
