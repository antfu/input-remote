'use strict';

var Keyboard = function(sendkeyfunc)
{
  var kb = this;
  kb.sendkey = sendkeyfunc;

  var is_shift_down = false;
  var is_ctrl_down = false;
  var is_alt_down = false;

  // Keyboard Map Objects
  kb.numpad = {
    keys:["123","456","789","0"],
    row:4,
    span:[0,0,0,2],
    target:'.keyboard.numpad'
  }
  kb.qwert_full = {
    keys:[
      "1234567890-=",
      "QWERTYUIOP[]",
      "ASDFGHJKL;''",
      [
        {code:16,key:'Shift',display:'<i class="icon arrow up"></i>',width:1.5},
        "ZXCVBNM,./",
        {code:8,key:'Backspace',display:'<i class="icon arrow circle left"></i>',width:1.5}
      ]
    ],
    span:[0,1,2,0],
    row:4,
    target:'.keyboard.qwert'
  }
  kb.qwert = {
    keys:[
      "QWERTYUIOP",
      "ASDFGHJKL",
      "ZXCVBNM"
    ],
    span:[0,1,2],
    row:3,
    target:'.keyboard.qwert'
  }
  kb.func = {
    keys:[[
      {code:38,key:"↑"}
    ],[
      {code:37,key:"←"},
      {code:40,key:"↓"},
      {code:39,key:"→"}
    ],[
    ]],
    span:[2,0,0],
    row:3,
    target:'.keyboard.functional'
  }

  kb.register_key_event = function(keys) {
    keys.each(function(i,e) {
      e = $(e);
      e.mousedown(function(){
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
      e.mouseup(function(){
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
      e.mouseleave(function(){
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
    for (var r=0; r<keymap.row; r++)
    {
      if (keymap.span)
        for (var s=0; s<keymap.span[r]; s++)
          target.append('<span></span>')
      var keys = keymap.keys[r];
      if (typeof keys == "string")
        keys = [keys];
      for (var c=0; c<keys.length; c++)
      {
        var obj = keys[c];
        if (typeof obj == "string")
          for (var k=0; k<obj.length; k++)
            target.append('<key code="'+obj.charCodeAt(k)+'" key="'+obj[k]+'">'+obj[k]+'</key>');
        else
          target.append('<key code="'+obj.code+'" key="'+obj.key+'" style="width:'+(32*(obj.width||1))+'px">'+(obj.display||obj.key)+'</key>')
      }
      target.append('<br>')
    }
  }

  return kb;
}
