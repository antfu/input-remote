'use strict';

init();
var keyboard;
var touchpad;

var mouse_speed = 3;
// Disable scroll
document.body.addEventListener('touchmove', function(event) {
  event.preventDefault();
}, false);

$('#toggle-keyboard').click(function() {
  if ($(this).hasClass('active'))
  {
    $('#keyboard-dock').removeClass('hidden');
    switch_keyboards('qwert',$('.breadcrumb .section:first'))
  }
  else
  {
    $('#keyboard-dock').addClass('hidden');
    switch_keyboards(null,'')
  }
});
$('#toggle-touchpad').click(function() {
  if ($(this).hasClass('active'))
  {
    $('.touchpad').removeClass('hidden');
    touchpad.start();
  }
  else
  {
    $('.touchpad').addClass('hidden');
    touchpad.stop();
  }
});

function switch_keyboards(key,bread) {
  $('.keyboard').hide();
  $('.breadcrumb .section').removeClass('active');
  $(bread).addClass('active');
  if(key)
      $('.keyboard.'+key).show();
}
switch_keyboards(null,$('#none_keyboad'));

var pressed = {};
var ws_url = 'ws://'+location.host+':81/ws/s';
var client = new WSClient(ws_url,true);
client.connect();
client.onstatechange = function(state) {
  $('.nav .state').html(state);
}
function sendkey(event) {
  var keycode = event.keyCode;
  var obj = {};
  obj.action = 'key';
  obj.subaction = event.type;
  obj.data = {};
  obj.data.keyaction = event.type;
  obj.data.key = event.key;
  obj.data.keycode = event.keyCode;
  obj.data.is_shift_down = event.shiftKey;
  obj.data.is_ctrl_down = event.ctrlKey;
  obj.data.is_alt_down = event.altKey;
  client.sendkey(obj);
  console.log(event.type + ': ' + obj.data.key);
}

var lastmousemove = {x:0,y:0};
function sendmouse(type,touch_obj) {
  var obj = {};
  var data = {};
  if (touch_obj == undefined) touch_obj = {x:0,y:0,button:''};
  data.x = parseInt((touch_obj.x || 0) * mouse_speed);
  data.y = parseInt((touch_obj.y || 0) * mouse_speed);
  data.button = touch_obj.button || '';
  data.action = type;
  obj.action = 'mouse';
  obj.subaction = type;
  obj.data = data;

  if (type == 'move')
  {
    if (data.x == lastmousemove.x && data.y == lastmousemove.y)
      return;
    lastmousemove.x = data.x;
    lastmousemove.y = data.y;
  }

  client.sendkey(obj);
  console.log(type + ': ', obj.data);
}
document.addEventListener('keydown', function(event) {
  if (!$('#toggle-physical').hasClass('active')) return;
  sendkey(event);
  event.preventDefault();
  return false;
});
document.addEventListener('keyup', function(event) {
  if (!$('#toggle-physical').hasClass('active')) return;
  sendkey(event);
  event.preventDefault();
  return false;
});

$(function(){
  keyboard = new Keyboard(sendkey,40);
  keyboard.make(keyboard.qwert);
  //keyboard.make(keyboard.qwert_full);
  keyboard.make(keyboard.numpad);
  keyboard.make(keyboard.func);
  keyboard.make(keyboard.symbol);
  keyboard.make(keyboard.media);
  keyboard.make(keyboard.navi);
  keyboard.register_key_event($('.keyboard key'));
  keyboard.vibrate = function() {
    if ($('#toggle-vibration').hasClass('active'))
      navigator.vibrate(40);
  };
  touchpad = new Touchpad($('#touchpad'), sendmouse);
  touchpad.on_state_change = function (icon,text) {
    $('.touchpad .background i').text(icon);
    $('.touchpad .background span').text(text);
  };

  if ($(window).width() <= 500)
  {
    $('#toggle-keyboard, #toggle-touchpad').click();
  }
  else {
    $('#toggle-physical').click();
  }
});
