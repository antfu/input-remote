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
from   configs.config   import configs

senders = {}
recivers = {}
selector = {'s':senders,'r':recivers}
invertor = {'r':senders,'s':recivers}

class index_handler(tornado.web.RequestHandler):
    def get(self):
        self.render('index.html')

class sender_handler(tornado.web.RequestHandler):
    def get(self):
        self.render('sender.html')

class receiver_handler(tornado.web.RequestHandler):
    def get(self):
        self.render('receiver.html')

class ws_handler(tornado.websocket.WebSocketHandler):
    def open(self, client_type):
        self.channel_id = self.get_argument('channel_id','default')
        self.type = client_type
        if self.channel_id in selector[self.type].keys():
            self.try_close()
            return
        selector[self.type][self.channel_id] = self
        if self.channel_id in invertor[self.type].keys():
            invertor[self.type][self.channel_id].send_message(dict(online=True))
            self.send_message(dict(online=True))

    def on_close(self,*args,**kwargs):
        del(selector[self.type][self.channel_id])
        invertor[self.type][self.channel_id].send_message(dict(online=False))

    def on_message(self, message):
        if self.channel_id in invertor[self.type].keys():
            invertor[self.type][self.channel_id].write_message(message)

    def try_close(self):
        if self.ws_connection is not None:
            self.close()

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
            (r'/ws/(s)',ws_handler),
            (r'/ws/(r)',ws_handler),
            (r'/s',sender_handler),
            (r'/r',receiver_handler),
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
