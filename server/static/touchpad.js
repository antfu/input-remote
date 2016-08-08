'use strict';

var Touchpad = function(target, sendtouchfunc)
{
  /* Constants */
  var tap_theshold = 0.3;
  var right_theshold = 1;
  var move_theshold = 2;
  var dragmove_theshold = 0.3;
  var crosshair_length = 20;
  var crosshair_padding = 30;


  var tp = this;
  tp.state = false;
  tp.target = target;
  var w = target.width();
  var h = target.height();
  tp.target.attr('width',w+'px');
  tp.target.attr('height',h+'px');
  var ctx = tp.target[0].getContext('2d');
  var x = tp.target.position().left;
  var y = tp.target.position().top;
  var touches = [];
  var start_touches = [];
  var offsets = [];

  var canvas_updating = false;
  var start_time;
  var moved = false;
  var dragmove_timer = undefined;
  var draging = false;

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
    if (draging)
    {
      sendtouchfunc('buttonup',{button:'left'});
      draging = false;
    }
    if (!moved && end_time - start_time < tap_theshold)
    {
      dragmove_timer = setTimeout(function () {
        dragmove_timer = undefined;
        sendtouchfunc('buttondown',{button:'left'});
        sendtouchfunc('buttonup',{button:'left'});
      }, dragmove_theshold * 1000);
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
    canvas_update();
  });

  tp.target[0].addEventListener('touchstart', function(event) {
    if (!tp.state) return;
    start_touches = event.touches;
    start_time = Date.now() / 1000;
    moved = false;
    sendtouchfunc('start');
    if (dragmove_timer !== undefined)
    {
      clearTimeout(dragmove_timer);
      dragmove_timer = undefined;
      draging = true;
      sendtouchfunc('buttondown',{button:'left'});
    }
    //console.log('Touchstart:',event.touches.length);
  });

  var canvas_update = function() {
    if (!tp.state) return;
  	if (canvas_updating) return;
  	canvas_updating = true;

  	ctx.clearRect(0, 0, w, h);

  	var i, len = touches.length;
  	for (i=0; i<len; i++) {
  		var touch = touches[i];
      var px = touch.pageX - x;
      var py = touch.pageY - y;

      // Drawing touch dots
  		ctx.beginPath();
  		ctx.arc(px, py, draging?10:5, 0, 2*Math.PI, true);

  		ctx.fillStyle = "rgba(0, 0, 0, 0.1)";
  		ctx.fill();

  		ctx.strokeStyle = "rgba(0, 0, 0, 0.6)";
  		ctx.stroke();

      // Drawing cross hair
      var directions = [[0,1],[0,-1],[1,0],[-1,0]];
      $.each(directions,function(i,e) {
        ctx.beginPath();
        ctx.moveTo(px + crosshair_padding * e[0],py + crosshair_padding * e[1]);
        ctx.lineTo(px + (crosshair_padding + crosshair_length) * e[0],py + (crosshair_padding + crosshair_length) * e[1]);
        ctx.strokeStyle = "rgba(0, 0, 0, 0.3)";
        ctx.stroke();
      });
  	}
  	canvas_updating = false;
  }

  var send_offset = function(){
    if (offsets.length > 0)
      sendtouchfunc('move',offsets[0]);
  }

  //var timer = setInterval(update, 15);
  var move_timer = setInterval(send_offset, 30);
}
