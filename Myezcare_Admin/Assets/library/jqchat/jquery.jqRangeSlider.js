/*
 * jqRangeSlider v3.8.5.0 (http://www.jqchart.com)
 * Copyright 2013 jqChart Inc. All rights reserved.
 *
 * jqRangeSlider commercial licenses may be obtained at
 * http://www.jqchart.com/pricing.aspx
 * If you do not own a commercial license, this file shall be governed by the trial license terms.
 */
(function(a){a.fn.jqRangeSlider=function(c,e,f){if(typeof c==="object")e=c;else if(typeof c==="string"){c=c.toLowerCase();if(a.fn.jqRangeSlider.methods[c])return a.fn.jqRangeSlider.methods[c].call(this,e,f);else a.error("Method "+method+" does not exist on jQuery.jqRangeSlider")}var d=this.data("data");if(!d){d=new b(this);this.data("data",d)}d._setOptions(e);return this};a.fn.jqRangeSlider.methods={rangeslider:function(){return this.data("data")},destroy:function(){var a=this.data("data");if(a){a.destroy();this.removeData("data")}},options:function(){var a=this.data("data");return!a?void 0:a.options},option:function(b,c){var a=this.data("data");if(!a)return;if(!c)return a.options[b];a.options[b]=c;a._setOptions(a.options)},update:function(c){var b=this.data("data");if(!b)return this;var d=a.extend(false,{},b.options,c||{});b._setOptions(d);return this},preview:function(){var a=this.data("data");return a?a.orientation=="horizontal"?a.bgHor:a.bgVer:null}};a.fn.jqRangeSlider.defaults={orientation:"horizontal",mode:"normal",minimum:0,maximum:100,minRange:0,range:{minimum:null,maximum:null},smallChange:10,largeChange:20,reversed:false};a.fn.jqMouseCapture=function(b){var c=a(document);this.each(function(){var d=a(this),e={};d.mousedown(function(h){var f;if(b.move){f=function(a){b.move.call(d,a,e)};c.mousemove(f)}var a,g=function(){b.move&&c.unbind("mousemove",f);c.unbind("mouseup",a)};if(b.up)a=function(a){g();return b.up.call(d,a,e)};else a=g;c.mouseup(a);h.preventDefault();return b.down.call(d,h,e)})});return this};var c=function(){return window.requestAnimationFrame||window.webkitRequestAnimationFrame||window.mozRequestAnimationFrame||window.oRequestAnimationFrame||window.msRequestAnimationFrame||function(a){return window.setTimeout(function(){a()},25)}}();function d(f){var d=f,a=false,b=false;function e(){if(a){d();c(e);b=true;a=false}else b=false}this.kick=function(){a=true;!b&&e()};this.end=function(c){var e=d;if(!c)return;if(!b)c();else{d=a?function(){e();c()}:c;a=true}}}function b(b){this._init(b);this.isTouchDevice=!!("ontouchstart"in window);this.timer=new d(a.proxy(this._arrangeElements,this))}b.prototype={_init:function(a){this.elem=a;a.addClass("ui-jqrangeslider ui-widget");a.css("position")=="static"&&a.css("position","relative")},_createElements:function(){var a=this.elem;a.children().remove();if(this.orientation=="horizontal")this._createHorElements(a);else this._createVerElements(a)},_createHorElements:function(c){var b=this;this.bgHor=a("<div class='ui-jqrangeslider-background-horizontal'></div>").css("top",0).css("left",0);c.append(this.bgHor);this.bgHorLeft=a("<div class='ui-jqrangeslider-background-horizontal-left'></div>").css("top",0).css("left",0).jqMouseCapture({down:function(a){b._largeScrollStart(a);b.scrolling=true;b.scrollingCount=0;b._scrollLeft()},up:a.proxy(b._stopScroll,this)});c.append(this.bgHorLeft);this.bgHorRight=a("<div class='ui-jqrangeslider-background-horizontal-right'></div>").css("top",0).css("left",0).jqMouseCapture({down:function(a){b._largeScrollStart(a);b.scrolling=true;b.scrollingCount=0;b._scrollRight()},up:a.proxy(b._stopScroll,this)});c.append(this.bgHorRight);this.arrowLeft=a("<div class='ui-jqrangeslider-arrow-left ui-corner-left ui-widget-header'></div>").css("top",0).css("left",0).jqMouseCapture({down:function(){b.scrolling=true;b.scrollingCount=0;b._scrollLeft()},up:a.proxy(b._stopScroll,this)});c.append(this.arrowLeft);this._addHover(this.arrowLeft);this.arrowIconLeft=a("<span class='ui-jqrangeslider-arrow-icon-left ui-icon ui-icon-circle-triangle-w'></span>");this.arrowLeft.append(this.arrowIconLeft);this.arrowRight=a("<div class='ui-jqrangeslider-arrow-right ui-corner-right ui-widget-header'></div>").css("top",0).css("left",0).jqMouseCapture({down:function(){b.scrolling=true;b.scrollingCount=0;b._scrollRight()},up:a.proxy(b._stopScroll,this)});c.append(this.arrowRight);this._addHover(this.arrowRight);this.arrowIconRight=a("<span class='ui-jqrangeslider-arrow-icon-right ui-icon ui-icon-circle-triangle-e'></span>");this.arrowRight.append(this.arrowIconRight);this.sliderHor=a("<div class='ui-jqrangeslider-slider-horizontal'></div>").css("top",0).css("left",0).jqMouseCapture({down:a.proxy(b._startDragHor,this),move:a.proxy(b._draggingHor,this),up:a.proxy(b._stopDragHor,this)});c.append(this.sliderHor);this.handleLeft=a("<div class='ui-jqrangeslider-handle-left ui-state-default'></div>").css("top",0).css("left",0).jqMouseCapture({down:a.proxy(b._startResizeLeft,this),move:a.proxy(b._resizeLeft,this),up:a.proxy(b._stopResizeLeft,this)});c.append(this.handleLeft);this._addHover(this.handleLeft);this.handleIconLeft=a("<span class='ui-jqrangeslider-handle-icon-left ui-icon ui-icon-grip-solid-vertical'></span>");this.handleLeft.append(this.handleIconLeft);this.handleRight=a("<div class='ui-jqrangeslider-handle-right ui-state-default'></div>").css("top",0).css("left",0).jqMouseCapture({down:a.proxy(b._startResizeRight,this),move:a.proxy(b._resizeRight,this),up:a.proxy(b._stopResizeRight,this)});c.append(this.handleRight);this._addHover(this.handleRight);this.handleIconRight=a("<span class='ui-jqrangeslider-handle-icon-right ui-icon ui-icon-grip-solid-vertical'></span>");this.handleRight.append(this.handleIconRight);if(this.mode!="preview"){this.bgHorLeft.addClass("ui-widget-content");this.bgHorRight.addClass("ui-widget-content");this.sliderHor.addClass("ui-widget-header")}if(this.isTouchDevice){this.bgHorLeft.bind("touchstart",function(a){a.preventDefault();b._largeScrollStart(a,true);b.scrolling=true;b.scrollingCount=0;b._scrollLeft()}).bind("touchend",function(){b._stopScroll()});this.bgHorRight.bind("touchstart",function(a){a.preventDefault();b._largeScrollStart(a,true);b.scrolling=true;b.scrollingCount=0;b._scrollRight()}).bind("touchend",function(){b._stopScroll()});this.arrowLeft.bind("touchstart",function(a){a.preventDefault();b.scrolling=true;b.scrollingCount=0;b._scrollLeft()}).bind("touchend",function(){b._stopScroll()});this.arrowRight.bind("touchstart",function(a){a.preventDefault();b.scrolling=true;b.scrollingCount=0;b._scrollRight()}).bind("touchend",function(){b._stopScroll()});this.sliderHor.bind("touchstart",function(a){b._startDragHor(a,true)}).bind("touchmove",function(a){b._draggingHor(a,true)}).bind("touchend",function(a){b._stopDragHor(a)});this.handleLeft.bind("touchstart",function(a){b._startResizeLeft(a,true)}).bind("touchmove",function(a){b._resizeLeft(a,true)}).bind("touchend",function(a){b._stopResizeLeft(a)});this.handleRight.bind("touchstart",function(a){b._startResizeRight(a,true)}).bind("touchmove",function(a){b._resizeRight(a,true)}).bind("touchend",function(a){b._stopResizeRight(a)})}},_createVerElements:function(c){var b=this;this.bgVer=a("<div class='ui-jqrangeslider-background-vertical'></div>").css("top",0).css("left",0);c.append(this.bgVer);this.bgVerBottom=a("<div class='ui-jqrangeslider-background-vertical-bottom'></div>").css("top",0).css("left",0).jqMouseCapture({down:function(a){b._largeScrollStart(a);b.scrolling=true;b.scrollingCount=0;b._scrollLeft()},up:a.proxy(b._stopScroll,this)});c.append(this.bgVerBottom);this.bgVerTop=a("<div class='ui-jqrangeslider-background-vertical-top'></div>").css("top",0).css("left",0).jqMouseCapture({down:function(a){b._largeScrollStart(a);b.scrolling=true;b.scrollingCount=0;b._scrollRight()},up:a.proxy(b._stopScroll,this)});c.append(this.bgVerTop);this.arrowBottom=a("<div class='ui-jqrangeslider-arrow-bottom ui-corner-bottom ui-widget-header'></div>").css("top",0).css("left",0).jqMouseCapture({down:function(){b.scrolling=true;b.scrollingCount=0;b._scrollLeft()},up:a.proxy(b._stopScroll,this)});c.append(this.arrowBottom);this._addHover(this.arrowBottom);this.arrowIconBottom=a("<span class='ui-jqrangeslider-arrow-icon-bottom ui-icon ui-icon-circle-triangle-s'></span>");this.arrowBottom.append(this.arrowIconBottom);this.arrowTop=a("<div class='ui-jqrangeslider-arrow-top ui-corner-top ui-widget-header'></div>").css("top",0).css("left",0).jqMouseCapture({down:function(){b.scrolling=true;b.scrollingCount=0;b._scrollRight()},up:a.proxy(b._stopScroll,this)});c.append(this.arrowTop);this._addHover(this.arrowTop);this.arrowIconTop=a("<span class='ui-jqrangeslider-arrow-icon-top ui-icon ui-icon-circle-triangle-n'></span>");this.arrowTop.append(this.arrowIconTop);this.sliderVer=a("<div class='ui-jqrangeslider-slider-vertical'></div>").css("top",0).css("left",0).jqMouseCapture({down:a.proxy(b._startDragVer,this),move:a.proxy(b._draggingVer,this),up:a.proxy(b._stopDragVer,this)});c.append(this.sliderVer);this.handleBottom=a("<div class='ui-jqrangeslider-handle-bottom ui-state-default'></div>").css("top",0).css("left",0).jqMouseCapture({down:a.proxy(b._startResizeBottom,this),move:a.proxy(b._resizeBottom,this),up:a.proxy(b._stopResizeBottom,this)});c.append(this.handleBottom);this._addHover(this.handleBottom);this.handleIconBottom=a("<span class='ui-jqrangeslider-handle-icon-bottom ui-icon ui-icon-grip-solid-horizontal'></span>");this.handleBottom.append(this.handleIconBottom);this.handleTop=a("<div class='ui-jqrangeslider-handle-top ui-state-default'></div>").css("top",0).css("left",0).jqMouseCapture({down:a.proxy(b._startResizeTop,this),move:a.proxy(b._resizeTop,this),up:a.proxy(b._stopResizeTop,this)});c.append(this.handleTop);this._addHover(this.handleTop);this.handleIconTop=a("<span class='ui-jqrangeslider-handle-icon-top ui-icon ui-icon-grip-solid-horizontal'></span>");this.handleTop.append(this.handleIconTop);if(this.mode!="preview"){this.bgVerBottom.addClass("ui-widget-content");this.bgVerTop.addClass("ui-widget-content");this.sliderVer.addClass("ui-widget-header")}if(this.isTouchDevice){this.bgVerBottom.bind("touchstart",function(a){a.preventDefault();b._largeScrollStart(a,true);b.scrolling=true;b.scrollingCount=0;b._scrollLeft()}).bind("touchend",function(){b._stopScroll()});this.bgVerTop.bind("touchstart",function(a){a.preventDefault();b._largeScrollStart(a,true);b.scrolling=true;b.scrollingCount=0;b._scrollRight()}).bind("touchend",function(){b._stopScroll()});this.arrowBottom.bind("touchstart",function(a){a.preventDefault();b.scrolling=true;b.scrollingCount=0;b._scrollLeft()}).bind("touchend",function(){b._stopScroll()});this.arrowTop.bind("touchstart",function(a){a.preventDefault();b.scrolling=true;b.scrollingCount=0;b._scrollRight()}).bind("touchend",function(){b._stopScroll()});this.sliderVer.bind("touchstart",function(a){b._startDragVer(a,true)}).bind("touchmove",function(a){b._draggingVer(a,true)}).bind("touchend",function(a){b._stopDragVer(a)});this.handleBottom.bind("touchstart",function(a){b._startResizeBottom(a,true)}).bind("touchmove",function(a){b._resizeBottom(a,true)}).bind("touchend",function(a){b._stopResizeBottom(a)});this.handleTop.bind("touchstart",function(a){b._startResizeTop(a,true)}).bind("touchmove",function(a){b._resizeTop(a,true)}).bind("touchend",function(a){b._stopResizeTop(a)})}},_setOptions:function(d){var b=a.extend(true,{},a.fn.jqRangeSlider.defaults,d||{});this.options=b;if(!b.range)b.range={};if(this.orientation!=b.orientation||this.mode!=b.mode){this.orientation=b.orientation;this.mode=b.mode;this._createElements()}var c=b.range;c.minimum=c.minimum===null||c.minimum===undefined?b.minimum:c.minimum;c.maximum=c.maximum===null||c.maximum===undefined?b.maximum:c.maximum;this.lastChange=a.extend({},c);this._arrange()},_arrange:function(){this.timer.kick()},_arrangeElements:function(){if(this.orientation=="horizontal")this._arrangeHor();else this._arrangeVer()},_arrangeHor:function(){var k=this.options,l=k.range,m=this.elem,i=m.width(),c=m.height(),g=this.arrowLeft.outerWidth(),f=this.arrowRight.outerWidth(),d=this.handleLeft.outerWidth(),j=this.handleRight.outerWidth();this._outerH(this.arrowLeft,c);this._outerH(this.arrowRight,c).css("left",i-f);this.arrowIconLeft.css({top:(this.arrowLeft.height()-this.arrowIconLeft.height())/2,left:(this.arrowLeft.width()-this.arrowIconLeft.width())/2});this.arrowIconRight.css({top:(this.arrowRight.height()-this.arrowIconRight.height())/2,left:(this.arrowRight.width()-this.arrowIconRight.width())/2});var a=g,b=i-a-f-d-j;this.innerW=b;this._outerH(this.bgHor,c).css("left",a+d);this._outerW(this.bgHor,b);var e=Math.round(this._map(l.minimum,b)),h=Math.round(b-this._map(l.maximum,b));if(k.reversed){var n=e;e=h;h=n}a+=e;b-=e+h;this._outerH(this.handleLeft,c).css("left",a);this._outerH(this.handleRight,c).css("left",a+b+d);this.handleIconLeft.css({top:(this.handleLeft.height()-this.handleIconLeft.height())/2,left:(this.handleLeft.width()-this.handleIconLeft.width())/2});this.handleIconRight.css({top:(this.handleRight.height()-this.handleIconRight.height())/2,left:(this.handleRight.width()-this.handleIconRight.width())/2});this._outerH(this.sliderHor,c).css("left",a+d);this._outerW(this.sliderHor,b);this._outerH(this.bgHorLeft,c).css("left",g);this._outerW(this.bgHorLeft,a-g);a+=b+d+j;this._outerH(this.bgHorRight,c).css("left",a);this._outerW(this.bgHorRight,i-a-f)},_arrangeVer:function(){var k=this.options,l=k.range,m=this.elem,c=m.width(),i=m.height(),f=this.arrowBottom.outerHeight(),g=this.arrowTop.outerHeight(),j=this.handleBottom.outerHeight(),d=this.handleTop.outerHeight();this._outerW(this.arrowBottom,c).css("top",i-f);this._outerW(this.arrowTop,c);this.arrowIconBottom.css({top:(this.arrowBottom.height()-this.arrowIconBottom.height())/2,left:(this.arrowBottom.width()-this.arrowIconBottom.width())/2});this.arrowIconTop.css({top:(this.arrowTop.height()-this.arrowIconTop.height())/2,left:(this.arrowTop.width()-this.arrowIconTop.width())/2});var a=g,b=i-a-f-j-d;this.innerH=b;this._outerW(this.bgVer,c).css("top",a+d);this._outerH(this.bgVer,b);var h=Math.round(this._map(l.minimum,b)),e=Math.round(b-this._map(l.maximum,b));if(k.reversed){var n=h;h=e;e=n}a+=e;b-=h+e;this._outerW(this.handleTop,c).css("top",a);this._outerW(this.handleBottom,c).css("top",a+b+d);this.handleIconBottom.css({top:(this.handleBottom.height()-this.handleIconBottom.height())/2,left:(this.handleBottom.width()-this.handleIconBottom.width())/2});this.handleIconTop.css({top:(this.handleTop.height()-this.handleIconTop.height())/2,left:(this.handleTop.width()-this.handleIconTop.width())/2});this._outerW(this.sliderVer,c).css("top",a+d);this._outerH(this.sliderVer,b);this._outerW(this.bgVerTop,c).css("top",g);this._outerH(this.bgVerTop,a-g);a+=b+j+d;this._outerW(this.bgVerBottom,c).css("top",a);this._outerH(this.bgVerBottom,i-a-f)},_map:function(f,c){var b=this.options,d=b.maximum-b.minimum,e=f-b.minimum,a=c*e/d;a=Math.min(a,c);a=Math.max(a,0);return a},_trigger:function(d,e){var b=this.options.range,c=this.lastChange;if(this.startMin===b.minimum&&this.startMax===b.maximum)return;if(!e&&c.minimum===b.minimum&&c.maximum===b.maximum)return;this.startMin=null;this.startMax=null;this.elem.trigger(d,{minimum:b.minimum,maximum:b.maximum});a.extend(c,b)},_startChange:function(){this.valueMin=this.startMin=this.options.range.minimum;this.valueMax=this.startMax=this.options.range.maximum},_largeScrollStart:function(c,h){if(h&&c.originalEvent.touches)c=c.originalEvent.touches[0];var i=c.pageX,j=c.pageY,e=this.elem.offset(),f=i-e.left,g=j-e.top,a=this.options,d=a.maximum-a.minimum,b;if(this.orientation=="horizontal"){f-=parseFloat(this.bgHor.css("left"));b=a.minimum+d*f/this.innerW}else{g-=parseFloat(this.bgVer.css("top"));b=a.maximum-d*g/this.innerH}if(a.reversed)b=a.minimum+a.maximum-b;this.endScrollValue=b},_moveLeft:function(){var a=this.options,d=a.range.maximum-a.range.minimum,b=a.range.minimum-a.smallChange;b=Math.max(a.minimum,b);var c=b+d;a.range.minimum=b;a.range.maximum=c;this._arrange();this._trigger("rangeChanged");if(this.endScrollValue)if((b+c)/2<this.endScrollValue)return true;return false},_moveRight:function(){var a=this.options,d=a.range.maximum-a.range.minimum,b=a.range.maximum+a.smallChange;b=Math.min(a.maximum,b);var c=b-d;a.range.minimum=c;a.range.maximum=b;this._arrange();this._trigger("rangeChanged");if(this.endScrollValue)if((c+b)/2>this.endScrollValue)return true;return false},_scrollLeft:function(){if(!this.scrolling)return;var b;if(!this.options.reversed)b=this._moveLeft();else b=this._moveRight();if(b){this.endScrollValue=null;return}var c=this.scrollingCount?100:200;this.scrollingCount++;setTimeout(a.proxy(this._scrollLeft,this),c)},_scrollRight:function(){if(!this.scrolling)return;var b;if(!this.options.reversed)b=this._moveRight();else b=this._moveLeft();if(b){this.endScrollValue=null;return}var c=this.scrollingCount?100:200;this.scrollingCount++;setTimeout(a.proxy(this._scrollRight,this),c)},_stopScroll:function(){this.scrolling=false},_startDragHor:function(a,b){a.preventDefault();if(b&&a.originalEvent.touches)a=a.originalEvent.touches[0];this.dragging=true;this.pos=a.pageX;this._startChange();this.sliderHor.addClass("ui-jqrangeslider-slider-horizontal-dragging")},_draggingHor:function(c,d){if(!this.dragging)return;c.preventDefault();if(d&&c.originalEvent.touches)c=c.originalEvent.touches[0];var f=c.pageX-this.pos,b=this.options,e=b.maximum-b.minimum,a=e*f/this.innerW;if(b.reversed)a=-a;a=Math.max(a,b.minimum-this.valueMin);a=Math.min(a,b.maximum-this.valueMax);b.range.minimum=this.valueMin+a;b.range.maximum=this.valueMax+a;this._arrange();this._trigger("rangeChanging")},_stopDragHor:function(a){a.preventDefault();this.dragging=false;this.sliderHor.removeClass("ui-jqrangeslider-slider-horizontal-dragging");this._trigger("rangeChanged",true);this.valueMin=null;this.valueMax=null},_startDragVer:function(a,b){a.preventDefault();if(b&&a.originalEvent.touches)a=a.originalEvent.touches[0];this.dragging=true;this.pos=a.pageY;this._startChange();this.sliderVer.addClass("ui-jqrangeslider-slider-vertical-dragging")},_draggingVer:function(c,d){if(!this.dragging)return;c.preventDefault();if(d&&c.originalEvent.touches)c=c.originalEvent.touches[0];var f=this.pos-c.pageY,b=this.options,e=b.maximum-b.minimum,a=e*f/this.innerH;if(b.reversed)a=-a;a=Math.max(a,b.minimum-this.valueMin);a=Math.min(a,b.maximum-this.valueMax);b.range.minimum=this.valueMin+a;b.range.maximum=this.valueMax+a;this._arrange();this._trigger("rangeChanging")},_stopDragVer:function(a){a.preventDefault();this.dragging=false;this.sliderVer.removeClass("ui-jqrangeslider-slider-vertical-dragging");this._trigger("rangeChanged",true);this.valueMin=null;this.valueMax=null},_startResizeLeft:function(a,b){a.preventDefault();if(b&&a.originalEvent.touches)a=a.originalEvent.touches[0];this.resize=true;this.pos=a.pageX;this._startChange()},_resizeLeft:function(c,e){if(!this.resize)return;c.preventDefault();if(e&&c.originalEvent.touches)c=c.originalEvent.touches[0];var g=c.pageX-this.pos,b=this.options,f=b.maximum-b.minimum,a=f*g/this.innerW,d=this.options.minRange;if(!b.reversed){a=Math.max(a,b.minimum-this.valueMin);a=Math.min(a,b.range.maximum-this.valueMin-d);b.range.minimum=this.valueMin+a}else{a=-a;a=Math.min(a,b.maximum-this.valueMax);a=Math.max(a,b.range.minimum-this.valueMax+d);b.range.maximum=this.valueMax+a}this._arrange();this._trigger("rangeChanging")},_stopResizeLeft:function(a){a.preventDefault();this.resize=false;this._trigger("rangeChanged",true);this.valueMin=null;this.valueMax=null},_startResizeBottom:function(a,b){a.preventDefault();if(b&&a.originalEvent.touches)a=a.originalEvent.touches[0];this.resize=true;this.pos=a.pageY;this._startChange()},_resizeBottom:function(c,e){if(!this.resize)return;c.preventDefault();if(e&&c.originalEvent.touches)c=c.originalEvent.touches[0];var g=this.pos-c.pageY,b=this.options,d=this.options.minRange,f=b.maximum-b.minimum,a=f*g/this.innerH;if(!b.reversed){a=Math.max(a,b.minimum-this.valueMin);a=Math.min(a,b.range.maximum-this.valueMin-d);b.range.minimum=this.valueMin+a}else{a=-a;a=Math.min(a,b.maximum-this.valueMax);a=Math.max(a,b.range.minimum-this.valueMax+d);b.range.maximum=this.valueMax+a}this._arrange();this._trigger("rangeChanging")},_stopResizeBottom:function(a){a.preventDefault();this.resize=false;this._trigger("rangeChanged",true);this.valueMin=null;this.valueMax=null},_startResizeRight:function(a,b){a.preventDefault();if(b&&a.originalEvent.touches)a=a.originalEvent.touches[0];this.resize=true;this.pos=a.pageX;this._startChange()},_resizeRight:function(c,e){if(!this.resize)return;c.preventDefault();if(e&&c.originalEvent.touches)c=c.originalEvent.touches[0];var g=c.pageX-this.pos,b=this.options,d=this.options.minRange,f=b.maximum-b.minimum,a=f*g/this.innerW;if(!b.reversed){a=Math.min(a,b.maximum-this.valueMax);a=Math.max(a,b.range.minimum-this.valueMax+d);b.range.maximum=this.valueMax+a}else{a=-a;a=Math.max(a,b.minimum-this.valueMin);a=Math.min(a,b.range.maximum-this.valueMin-d);b.range.minimum=this.valueMin+a}this._arrange();this._trigger("rangeChanging")},_stopResizeRight:function(a){a.preventDefault();this.resize=false;this._trigger("rangeChanged",true);this.valueMin=null;this.valueMax=null},_startResizeTop:function(a,b){a.preventDefault();if(b&&a.originalEvent.touches)a=a.originalEvent.touches[0];this.resize=true;this.pos=a.pageY;this._startChange()},_resizeTop:function(c,e){if(!this.resize)return;c.preventDefault();if(e&&c.originalEvent.touches)c=c.originalEvent.touches[0];var g=this.pos-c.pageY,b=this.options,d=this.options.minRange,f=b.maximum-b.minimum,a=f*g/this.innerH;if(!b.reversed){a=Math.min(a,b.maximum-this.valueMax);a=Math.max(a,b.range.minimum-this.valueMax+d);b.range.maximum=this.valueMax+a}else{a=-a;a=Math.max(a,b.minimum-this.valueMin);a=Math.min(a,b.range.maximum-this.valueMin-d);b.range.minimum=this.valueMin+a}this._arrange();this._trigger("rangeChanging")},_stopResizeTop:function(a){a.preventDefault();this.resize=false;this._trigger("rangeChanged",true);this.valueMin=null;this.valueMax=null},_outerH:function(c,b){var d=["Top","Bottom"];a.each(d,function(){b-=parseFloat(c.css("border"+this+"Width"))||0});b=Math.max(0,b);c.height(b);return c},_outerW:function(c,b){var d=["Left","Right"];a.each(d,function(){b-=parseFloat(c.css("border"+this+"Width"))||0});b=Math.max(0,b);c.width(b);return c},_addHover:function(b){b.hover(function(){a(this).addClass("ui-state-hover")},function(){a(this).removeClass("ui-state-hover")})},destroy:function(){var a=this.elem;a.children().remove();a.removeClass("ui-jqrangeslider ui-widget")}}})(jQuery)