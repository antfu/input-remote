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

class index_handler(tornado.web.RequestHandler):
    def get(self):
        self.render('index.html')

class websocket_handler(tornado.websocket.WebSocketHandler):
    def open(self):
        pass

    def on_close(self,*args,**kwargs):
        pass

    def on_message(self, message):
        pass

    def send_message(self,message):
        pass

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
            (r'/ws',websocket_handler)
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
