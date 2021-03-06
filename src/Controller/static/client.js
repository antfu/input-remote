'use strict';

var WSClient = function(url, debug, fake)
{
  var wsc = this;
  wsc.connected = false;
  wsc.online = false;
  wsc.url = url;
  wsc.debug = debug;
  wsc.fake = fake || false;
  wsc.get_state = function()
  {
    if (wsc.fake)
    {
      return 'Fake';
    }
    else if (!wsc.connected)
    {
      change_favicon('/static/icons/black_red.png');
      return 'Disconnect';
    } else if (!wsc.online) {
      change_favicon('/static/icons/black_grey.png');
      return '<div class="ui active mini inline loader"></div> Waiting';
    } else {
      change_favicon('/static/icons/black_green.png');
      return 'Online';
    }
  }
  wsc.connect = function()
  {
    if (wsc.fake)
    {
      console.log('[Controller] Started in fake mode');
      if (wsc.onstatechange)
        wsc.onstatechange(wsc.get_state());
      return;
    }
    wsc.ws = new WebSocket(wsc.url);
    wsc.ws.onopen = function() {
      wsc.connected = true;
      if (wsc.onopen)
        wsc.onopen();
      if (wsc.onstatechange)
        wsc.onstatechange(wsc.get_state());
      if (wsc.debug)
        console.log('[Controller] Connection established: ',wsc.url);
    };
    wsc.ws.onclose = function() {
      wsc.connected = false;
      if (wsc.onclose)
        wsc.onclose();
      if (wsc.debug)
        console.log('[Controller] Connection lost: ',wsc.url);
      wsc.ws = undefined;
      wsc.connected = false;
      wsc.online = false;
      if (wsc.onstatechange)
        wsc.onstatechange(wsc.get_state());
    };
    wsc.ws.onmessage = function (evt) {
      var data = JSON.parse(evt.data);
      if (wsc.debug)
        console.log('[Controller] Received: ', data);

      if (data.action == "system")
      {
        if (data.subaction == "peerstate")
        {
          wsc.online = data.data.state;
          if (wsc.onstatechange)
            wsc.onstatechange(wsc.get_state());
        }
      }
      else if (data.action == "key")
      {
        wsc.onkey(data.data);
      }
    };
  }
  wsc.disconnect = function()
  {
    if (wsc.fake) return;
    if (wsc.ws == undefined) return;
    wsc.ws.close();
    wsc.ws = undefined;
    wsc.connected = false;
    wsc.onstatechange(false);
  }
  wsc.sendkey = function(obj)
  {
    if (wsc.fake) return;
    if (wsc.ws == undefined) return;
    wsc.ws.send(JSON.stringify(obj));
  }
  return wsc;
};
