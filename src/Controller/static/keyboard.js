'use strict';

var Keyboard = function(sendkeyfunc)
{
  var icons = {
    backspace: '<i class="mi">backspace</i>',
    enter: '<i class="mi">keyboard_return</i>',
    shift: '<i class="mi">arrow_upward</i>',
    caps: '<i class="mi">keyboard_capslock</i>',
    option: '<i class="mi">settings</i>',
    face: '<i class="mi">face</i>',
    up: '<i class="mi">keyboard_arrow_up</i>',
    down: '<i class="mi">keyboard_arrow_down</i>',
    left: '<i class="mi">keyboard_arrow_left</i>',
    right: '<i class="mi">keyboard_arrow_right</i>',
    volume_up: '<i class="mi">volume_up</i>',
    volume_down: '<i class="mi">volume_down</i>',
    volume_mute: '<i class="mi">volume_off</i>',
    media_next: '<i class="mi">skip_next</i>',
    media_prev: '<i class="mi">skip_previous</i>',
    media_pause: '<i class="mi">pause</i>',
  }

  var kb = this;
  kb.sendkey = sendkeyfunc;

  var is_shift_down = false;
  var is_ctrl_down = false;
  var is_alt_down = false;

  kb.vibrate = function() {}
  // Keyboard Map Objects
  kb.numpad = {
    keys:[
      ["+123",{code:8,key:'Backspace',display:icons.backspace}],
      "-456(",
      "*789)",
      ["/%0.",{code:10,key:'Enter',display:icons.enter,width:1}]],
    row:4,
    width:100/5,
    width_unit: "%",
    target:'.keyboard.numpad'
  }
  kb.qwert = {
    keys:[
      "QWERTYUIOP",
      [
        "ASDFGHJKL",
        {code:8,key:'Backspace',display:icons.backspace,width:0.75}
      ],[
        {code:0x14,key:'Caps Lock',display:icons.caps,width:1},
        "ZXCVBNM.",
        {code:10,key:'Enter',display:icons.enter,width:1},
      ],[
        {code:0x25,key:'Left',display:icons.left,width:1.5},
        {code:32,key:'Space',display:'<div class="space-key">Space<div>',width:7,noactive:true},
        {code:0x27,key:'Right',display:icons.right,width:1.5},
      ]
    ],
    span:[0,0.25,0,0],
    row:4,
    width:10,
    width_unit: "%",
    target:'.keyboard.qwert'
  }
  kb.qwert_full = {
    keys:[[
        "1234567890",
        {code:8,key:'Backspace',display:icons.backspace,width:1}
      ],[
        "QWERTYUIOP"
      ],[
        "ASDFGHJKL",
        {code:10,key:'Enter',display:icons.enter,width:1}
      ],[
        {code:16,key:'Shift',display:icons.shift,width:1.5},
        "ZXCVBNM",
        {code:38,key:'ArrowUp',display:icons.up,width:1.5},
      ],[
        {code:17,key:'Ctrl',width:1},
        {code:18,key:'Alt',width:1},
        {code:32,key:'Space',display:'<div class="space-key">Space<div>',width:6,noactive:true},
        {code:37,key:'ArrowLeft',display:icons.left,width:1},
        {code:40,key:'ArrowDown',display:icons.down,width:1},
        {code:39,key:'ArrowRight',display:icons.right,width:1},
      ]
    ],
    span:[0,0.5,1,0,0],
    row:5,
    width:100/11,
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
      {code:91,key:"LWin", display:'LWin'},
      {code:92,key:"RWin", display:'RWin'},
      {code:17,key:'Ctrl'},
      {code:16,key:'Shift'},
      {code:18,key:'Alt'},
      {code:32,key:"Space"},
      {code:8,key:'Backspace',display:icons.backspace}
    ]],
    row:3,
    width:100/7,
    width_unit: "%",
    target:'.keyboard.functional'
  }
  kb.media = {
    keys:[[
      {code:0xAD,key:"VolumeMute",display:icons.volume_mute},
      {code:0xAE,key:"VolumeDown",display:icons.volume_down},
      {code:0xAF,key:"VolumeUp",display:icons.volume_up},
      {code:0xB1,key:"MediaPrev",display:icons.media_prev},
      {code:0xB0,key:"MediaNext",display:icons.media_next},
      {code:0xB2,key:"MediaPause",display:icons.media_pause},
    ]],
    row:1,
    width:100/6,
    width_unit: "%",
    target:'.keyboard.media'
  }
  kb.navi = {
    keys:[[
      {code:0x26,key:'Up',display:icons.up,width:1},
      {width: 1},
      {code:33,key:"PgUp"},
      {code:36,key:"Home"},
      {code:45,key:"Ins"},
      {code:8,key:'Backspace',display:icons.backspace}
    ],[
      {code:0x25,key:'Left',display:icons.left,width:1},
      {code:0x28,key:'Down',display:icons.down,width:1},
      {code:0x27,key:'Right',display:icons.right,width:1},
      {code:34,key:"PgDn"},
      {code:35,key:"End"},
      {code:18,key:"Menu"},
      {code:46,key:"Del"}
    ]],
    span:[1,0],
    row:2,
    width:100/7,
    width_unit: "%",
    target:'.keyboard.navi'
  }
  kb.symbol = {
    keys:[
      "~!@#$%^&*()",
      ["[]{}|\\_+-=",{code:8,key:'Backspace',display:icons.backspace}],
      [";:'\",.<>?/",{code:10,key:'Enter',display:icons.enter,width:1}]
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
        kb.vibrate();
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
