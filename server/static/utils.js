
'use strict';

/*=== Url parameter ===*/
function get_url_parameter(sParam) {
  var sPageURL = decodeURIComponent(window.location.search.substring(1)),
    sURLVariables = sPageURL.split('&'),
    sParameterName,
    i;
  for (i = 0; i < sURLVariables.length; i++) {
    sParameterName = sURLVariables[i].split('=');
    if (sParameterName[0] === sParam) {
      return sParameterName[1] === undefined ? true : sParameterName[1];
    }
  }
}

function make_qrcode(target_id,data) {
  (new QRCode(target_id,{
    width: 128,
    height: 128,
    correctLevel : QRCode.CorrectLevel.L
  })).makeCode(data);
}

function init_header(type) {
  if (type=="sender")
  {
    $('[name="type"]').text('Sender');
    var link = location.href.replace('sender','receiver');
  } else {
    $('[name="type"]').text('Receiver');
    var link = location.href.replace('receiver','sender');
  }
  $('#link').val(link);
  make_qrcode('qrcode',link);
  var link_copy = $('#link_copy');
  link_copy.attr('data-clipboard-text',link);
  (new Clipboard('#link_copy')).on('success', function(e){
    link_copy.removeClass('linkify').addClass('checkmark');
    setTimeout(function(){
      link_copy.removeClass('checkmark').addClass('linkify');
    },2000);
  });
  $('#qrcode_display').popup({popup: '#qrcode_popup'});
  $('#link_copy').popup();
}

function change_favicon(src) {
  var link = document.createElement('link'),
      oldLink = document.getElementById('favion');
  link.id = 'favion';
  link.rel = 'shortcut icon';
  link.href = src;
  if (oldLink) {
    document.head.removeChild(oldLink);
  }
  document.head.appendChild(link);
}
