'use strict';

var Touchpad = function(target,vibrate)
{
  var tp = this;
  tp.target = target;
  tp.vibrate = vibrate || 0;
	var touches = [];
  var w = 300;
  var h = 300;
  tp.target.width(w);
  tp.target.height(h);
  tp.target[0].style.width = w+"px";
  tp.target[0].style.height = h+"px";
  var ctx = tp.target[0].getContext('2d');
  var x = tp.target.position().left;
  var y = tp.target.position().top;
  var updateStarted = false ;

  tp.target[0].addEventListener('touchend', function() {
  	ctx.clearRect(0, 0, w, h);
    touches = [];
  });

  tp.target[0].addEventListener('touchmove', function(event) {
    event.preventDefault();
    touches = event.touches;
    console.log(touches);
  });

  tp.target[0].addEventListener('touchstart', function(event) {
    console.log('start');
  });

  var update = function() {
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

  		ctx.fillStyle = "rgba(0, 0, 0, 0.2)";
  		ctx.fill();

  		ctx.strokeStyle = "rgba(0, 0, 0, 0.8)";
  		ctx.stroke();
      console.log('drawn circle at ' + px +',' + py);
  	}

  	updateStarted = false;
  }

  var timer = setInterval(update, 15);
}
