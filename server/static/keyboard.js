'use strict';

var Keyboard = function(sendkeyfunc,vibrate)
{
  var kb = this;
  kb.sendkey = sendkeyfunc;
  kb.vibrate = vibrate || 0;

  var is_shift_down = false;
  var is_ctrl_down = false;
  var is_alt_down = false;

  // Keyboard Map Objects
  kb.numpad = {
    keys:[
      ["+123",{code:8,key:'Backspace',display:'<i class="icon arrow circle left"></i>'}],
      "-456(",
      "*789)",
      ["/%0.",{code:10,key:'Enter',display:'Enter',width:1}]],
    row:4,
    width:100/5,
    width_unit: "%",
    target:'.keyboard.numpad'
  }
  kb.qwert = {
    keys:[
      "QWERTYUIOP",
      "ASDFGHJKL",
      [
        {code:16,key:'Shift',display:'<i class="icon arrow up"></i>',width:1},
        "ZXCVBNM",
        {code:8,key:'Backspace',display:'<i class="icon arrow circle left"></i>',width:1.5}
      ],[
        {display:'@#$',width:1},
        {display:'<i class="icon smile"></i>',width:1},
        {code:32,key:'Space',display:'<div class="space-key">Space<div>',width:6,noactive:true},
        {code:10,key:'Enter',display:'Enter',width:1},
        {display:'<i class="icon options"></i>',width:1}
      ]
    ],
    span:[0,0.5,0,0],
    row:4,
    width:10,
    width_unit: "%",
    target:'.keyboard.qwert'
  }
  kb.qwert_full = {
    keys:[[
        "`1234567890-=",
        {code:8,key:'Backspace',display:'<i class="icon arrow circle left"></i>',width:1.5}
      ],[
        "QWERTYUIOP[]\\"
      ],[
        "ASDFGHJKL;''",
        {code:10,key:'Enter',display:'Enter',width:1}
      ],[
        {code:16,key:'Shift',display:'<i class="icon arrow up"></i>',width:1.5},
        "ZXCVBNM,./",
        {code:38,key:'ArrowUp',display:'<i class="icon arrow up"></i>',width:1.5},
      ],[
        {code:17,key:'Ctrl',width:1.5},
        {code:91,key:'Win',display:'<i class="icon windows"></i>',width:1.5},
        {code:18,key:'Alt',width:1},
        {code:32,key:'Space',display:'<div class="space-key">Space<div>',width:6,noactive:true},
        {code:37,key:'ArrowLeft',display:'<i class="icon arrow left"></i>',width:1.5},
        {code:40,key:'ArrowDown',display:'<i class="icon arrow down"></i>',width:1.5},
        {code:39,key:'ArrowRight',display:'<i class="icon arrow right"></i>',width:1.5},
      ]
    ],
    span:[0,0.5,1,0,0],
    row:5,
    width:100/15,
    width_unit: "%",
    target:'.keyboard.qwert-full'
  }
  kb.func = {
    keys:[[
      {code:27,key:"Esc"},
      {code:112,key:"F1"},
      {code:113,key:"F2"},
      {code:114,key:"F3"},
      {code:115,key:"F4"},
      {code:116,key:"F5"},
      {code:117,key:"F6"}
    ],[
      {code:9,key:"Tab"},
      {code:118,key:"F7"},
      {code:119,key:"F8"},
      {code:120,key:"F9"},
      {code:121,key:"F10"},
      {code:122,key:"F11"},
      {code:123,key:"F12"}
    ],[
      {code:91,key:"LWin", display:'L<i class="icon windows"></i>'},
      {code:92,key:"RWin", display:'R<i class="icon windows"></i>'},
      {code:17,key:'Ctrl'},
      {code:16,key:'Shift'},
      {code:18,key:'Alt'},
      {code:32,key:"Space"},
      {code:8,key:'Backspace',display:'<i class="icon arrow circle left"></i>'}
    ],[
      {code:45,key:"Ins"},
      {code:36,key:"Home"},
      {code:33,key:"PgUp"},
      {code:34,key:"PgDn"},
      {code:35,key:"End"},
      {code:18,key:"Menu"},
      {code:46,key:"Delete"}
    ]],
    row:4,
    width:100/7,
    width_unit: "%",
    target:'.keyboard.functional'
  }
  kb.symbol = {
    keys:[
      "~!@#$%^&*()",
      ["[]{}|\\_+-=",{code:8,key:'Backspace',display:'<i class="icon arrow circle left"></i>'}],
      [";:'\",.<>?/",{code:10,key:'Enter',display:'Enter',width:1}]
    ],
    row:3,
    width:100/11,
    width_unit: "%",
    target:'.keyboard.symbol'
  }

  kb.register_key_event = function(keys) {
    keys.each(function(i,e) {
      e = $(e);
      if (!e.attr('code'))
        return;
      e.on('mousedown',function(){
        navigator.vibrate(kb.vibrate);
        var event = {};
        event.type = "keydown";
        event.key = e.text();
        event.keyCode = e.attr('code');
        event.shiftKey = is_shift_down;
        event.ctrlKey = is_ctrl_down;
        event.altKey = is_alt_down;
        e.down = true;
        kb.sendkey(event);
      });
      e.on('mouseup',function(){
        var event = {};
        event.type = "keyup";
        event.key = e.text();
        event.keyCode = e.attr('code');
        event.shiftKey = is_shift_down;
        event.ctrlKey = is_ctrl_down;
        event.altKey = is_alt_down;
        e.down = false;
        kb.sendkey(event);
      });
      e.on('mouseleave',  function(){
        if (e.down)
        {
          var event = {};
          event.type = "keyup";
          event.key = e.text();
          event.keyCode = e.attr('code');
          event.shiftKey = is_shift_down;
          event.ctrlKey = is_ctrl_down;
          event.altKey = is_alt_down;
          kb.sendkey(event);
          e.down =false;
        }
      });
    })
  }
  kb.make = function (keymap) {
    var target = $(keymap.target).empty();
    var width = keymap.width || 10;
    var unit = keymap.width_unit || '%';

    for (var r=0; r<keymap.row; r++)
    {
      if (keymap.span)
          target.append('<div class="span" style="width:'+(width*(keymap.span[r]||0))+unit+'"></div>')
      var keys = keymap.keys[r];
      if (typeof keys == "string")
        keys = [keys];
      for (var c=0; c<keys.length; c++)
      {
        var obj = keys[c];

        if (typeof obj == "string")
        {
          for (var k=0; k<obj.length; k++)
            target.append('<key code="'+obj.charCodeAt(k)+'" key="'+obj[k]+'" style="width:'+width+unit+'">'+obj[k]+'</key>');
        }
        else
        {
          var special_key = $('<key class="special-key"></key>');
          if (obj.noactive)
            special_key.addClass('noactive');
          special_key.attr('code',obj.code);
          special_key.attr('key',obj.key);
          special_key.css('width',((obj.width||1)*width)+unit);
          special_key.html(obj.display||obj.key);
          if (obj.style)
          {
            var style_keys = Object.keys(obj.style);
            for (var k=0; k<style_keys.length; k++ )
              special_key.css(style_keys[k],obj.style[style_keys[k]]);
          }
          target.append(special_key);
        }
      }
      target.append('<br>')
    }
  }

  return kb;
}
