// ======================================================
//LOADER
// ======================================================
jQuery("#status").fadeOut();
jQuery(".preloader").delay(300).fadeOut("slow");
jQuery("body").css('overflow-y', 'visible');
jQuery("body").css('position', 'relative');
setTimeout(function(){
    jQuery('body').addClass('loaded');
}, 1000);

jQuery(function ($) {
    "use strict";

    var $window = $(window);
    var windowsize = $(window).width();
    var $root = $("html, body");
    var $body = $("body");

    // ======================================================
    //Equal Heights  Scroll
    // ======================================================
    checheight();
    $window.on("resize", function () {
        checheight();
    });
    function checheight() {
        var $smae_height = $(".equalheight");
        if ($smae_height.length) {
            if (windowsize > 359) {
                $smae_height.matchHeight({
                    property: "height"
                });
            }
        }
    }

  // ======================================================
  //Tables Sort out
  // ======================================================
  $(function(){
    $('table').tablesorter({
      widgets        : ['zebra', 'columns'],
      usNumberFormat : false,
      sortReset      : true,
      sortRestart    : true
    });
  });
  // ======================================================
  //Check Box
  // ======================================================
  $('.customers-packages-box').click(function ()
    {
    var checkbox = $(this).find('input[type=radio]');
    checkbox.prop("checked", !checkbox.prop("checked"));
  });

  $('.customers-packages').on('click', '.customers-packages-box-bg', function () {
      var self = $(this);
      if (self.hasClass('active')) {
          $('.customers-packages-box-bg').removeClass('active');
          return false;
      }
      $('.customers-packages-box-bg').removeClass('active');
      self.toggleClass('active');
      hide = false;
  });
  // ======================================================
  //Form
  // ======================================================
   $('input').focus(function(){
      $(this).parents('.form-group').addClass('focused');
    });

    $('input').blur(function(){
      var inputValue = $(this).val();
      if ( inputValue == "" ) {
        $(this).removeClass('filled');
        $(this).parents('.form-group').removeClass('focused');  
      } else {
        $(this).addClass('filled');
      }
    })  
  
});


// ======================================================
//Custom file upload
// ======================================================

function readURL(input) {
    if (input.files && input.files[0]) {
       
        var reader = new FileReader();

        reader.onload = function(e) {
            $('.image-upload-wrap').hide();

            $('.file-upload-image').attr('src', e.target.result);
            $('.file-upload-content').show();

            $('.image-title').html("/" + input.files[0].name);
        };

        reader.readAsDataURL(input.files[0]);

       
    } else {
        removeUpload();
    }
}



function removeUpload() {
    $('.file-upload-input').replaceWith($('.file-upload-input').clone());
    $('.file-upload-content').hide();
    $('.image-upload-wrap').show();

    $('.file-upload-image').attr('src', "");
    $('.image-title').html('');

 
}
$('.image-upload-wrap').bind('dragover', function () {
    $('.image-upload-wrap').addClass('image-dropping');
});
$('.image-upload-wrap').bind('dragleave', function () {
    $('.image-upload-wrap').removeClass('image-dropping');
});











