'use strict';

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
    });
    update();
  });

  tp.target[0].addEventListener('touchstart', function(event) {
    if (!tp.state) return;
    start_touches = event.touches;
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
