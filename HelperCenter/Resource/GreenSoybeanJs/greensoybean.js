(function($){
    Greensoybean = function(config){}

    /*
     * 判断浏览器的类型
     * @return 是否是指定的浏览器
     */
    Greensoybean.version = {
        firefox: /firefox/.test(navigator.userAgent.toLowerCase()),
        msie: /msie/.test(navigator.userAgent.toLowerCase()),
        opera: /opera/.test(navigator.userAgent.toLowerCase())
    }

    /*
     * 计算窗体居中的位置
     * @width 要显示窗体的宽度
     * @retion 返回一个json对象 {left : _left, top : _top}
    */
    Greensoybean.computeMiddlePosition = function(width){
        var winWidth = $(window).width();
        var winHeight = $(window).height();
        var meLeft = (winWidth - width) / 2;
        meLeft < 0 ? 0 : meLeft;
        meTop = $("html").scrollTop() + 100;
        return {left : meLeft, top : meTop};
    }

    /*
     * 防止事件冒泡
     * @e 事件对象
     */
    Greensoybean.stopBubble = function(e){
        if ( e && e.stopPropagation ) {
            // 因此它支持W3C的stopPropagation()方法
            e.stopPropagation();
        } else {
            // 否则，我们需要使用IE的方式来取消事件冒泡
            window.event.cancelBubble = true;
        }
    }

    /*
     * 初始化指定对象的拖动事件
     * @trigger 触发拖动的对象
     * @target 被拖动的对象
     */
    Greensoybean.initDrag = function(trigger, target){
        $(trigger).mousedown(function(e){
            if(e.button == 0){
                $(target).attr("data-move", "1");//处于移动状态
                var _offset = $(target).offset();
                _x = e.pageX - _offset.left;
                _y = e.pageY - _offset.top;
                $(target).fadeTo(500, 0.3);//开始移动的时候，设置不透明
            }
        });
        $(document).mousemove(function(e){
            if(e.button == 0){
                if($(target).attr("data-move") == "1"){
                    //移动过程中不停的计算最新坐标
                    var x = e.pageX - _x;
                    var y = e.pageY - _y;
                    $(target).css({
                        top: y,
                        left: x
                    });
                }
            }
        }).mouseup(function(e){
            if(e.button == 0){
                //加以下判断是为了，防止一个页面有多个移动源，移动后冲突
                if($(target).attr("data-move")==1){
                    $(target).removeAttr("data-move");//移动状态结束
                    $(target).fadeTo(500, 1);//松开鼠标后立刻停止移动并恢复成不透明
                }
            }
        });
    }
})(jQuery);