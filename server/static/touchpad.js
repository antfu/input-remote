'use strict';

var tap_theshold = 0.3;
var right_theshold = 1;
var move_theshold = 2;

var Touchpad = function(target, sendtouchfunc)
{
  var tp = this;
  tp.state = false;
  tp.target = target;
	var touches = [];
  var start_touches = [];
  var offsets = [];
  var w = target.width();
  var h = target.height();
  console.log('Touchpad',w,h);
  tp.target.attr('width',w+'px');
  tp.target.attr('height',h+'px');
  var ctx = tp.target[0].getContext('2d');
  var x = tp.target.position().left;
  var y = tp.target.position().top;
  var updateStarted = false;

  var start_time;
  var moved = false;

  tp.start = function() {
    tp.state = true;
  };
  tp.stop = function() {
    tp.state = false;
  };
  tp.target[0].addEventListener('touchend', function(event) {
    if (!tp.state) return;
  	ctx.clearRect(0, 0, w, h);
    touches = [];
    offsets = [];
    if (event.touches.length == 0)
      sendtouchfunc('end');
    var end_time = Date.now() / 1000;
    if (!moved && end_time - start_time < tap_theshold)
    {
      sendtouchfunc('buttondown',{button:'left'});
      sendtouchfunc('buttonup',{button:'left'});
    }
    if (!moved && end_time - start_time > right_theshold)
    {
      sendtouchfunc('buttondown',{button:'right'});
      sendtouchfunc('buttonup',{button:'right'});
    }
    moved = false;

    //console.log('Touchend:',event.touches.length);
  });

  tp.target[0].addEventListener('touchmove', function(event) {
    if (!tp.state) return;
    event.preventDefault();
    touches = event.touches;
    offsets = [];
    $.each(touches,function(i,e) {
      var x = e.pageX - start_touches[i].pageX;
      var y = e.pageY - start_touches[i].pageY;
      offsets.push({x:x,y:y});
      if (Math.abs(x) > move_theshold || Math.abs(y) > move_theshold)
        moved = true;
    });
    update();
  });

  tp.target[0].addEventListener('touchstart', function(event) {
    if (!tp.state) return;
    start_touches = event.touches;
    start_time = Date.now() / 1000;
    moved = false;
    sendtouchfunc('start');
    //console.log('Touchstart:',event.touches.length);
  });

  var update = function() {
    if (!tp.state) return;
  	if (updateStarted) return;
  	updateStarted = true;

  	ctx.clearRect(0, 0, w, h);

  	var i, len = touches.length;
  	for (i=0; i<len; i++) {
  		var touch = touches[i];
      var px = touch.pageX - x;
      var py = touch.pageY - y;

  		ctx.beginPath();
  		ctx.arc(px, py, 5, 0, 2*Math.PI, true);

  		ctx.fillStyle = "rgba(0, 0, 0, 0.1)";
  		ctx.fill();

  		ctx.strokeStyle = "rgba(0, 0, 0, 0.6)";
  		ctx.stroke();
  	}

  	updateStarted = false;
  }

  var send_offset = function(){
    if (offsets.length > 0)
      sendtouchfunc('move',offsets[0]);
  }

  //var timer = setInterval(update, 15);
  var move_timer = setInterval(send_offset, 30);
}
