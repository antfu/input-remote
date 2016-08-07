'use strict';

var Touchpad = function(target)
{
  var tp = this;
  tp.state = false;
  tp.target = target;
	var touches = [];
  var w = target.width();
  var h = target.height();
  console.log('Touchpad',w,h);
  tp.target.attr('width',w+'px');
  tp.target.attr('height',h+'px');
  var ctx = tp.target[0].getContext('2d');
  tp.ctx = ctx;
  var x = tp.target.position().left;
  var y = tp.target.position().top;
  var updateStarted = false ;

  tp.start = function() {
    tp.state = true;
  };
  tp.stop = function() {
    tp.state = false;
  };
  tp.target[0].addEventListener('touchend', function() {
    if (!tp.state) return;
  	ctx.clearRect(0, 0, w, h);
    touches = [];
  });

  tp.target[0].addEventListener('touchmove', function(event) {
    if (!tp.state) return;
    event.preventDefault();
    touches = event.touches;
    console.log(touches[0]);
  });

  tp.target[0].addEventListener('touchstart', function(event) {
    if (!tp.state) return;
    console.log('start');
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

  var timer = setInterval(update, 15);
}
