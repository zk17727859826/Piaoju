(function($){
    Greensoybean.dialog = function(config) {
        if (typeof gsb_dialog_object != "undefined" && gsb_dialog_object["gsb-dialog-object-" + config.contentid]) {
            return gsb_dialog_object["gsb-dialog-object-" + config.contentid];
        }
        if (typeof gsb_mix_dialog == "undefined"){
            gsb_mix_dialog = new Array();
        }
        if(typeof gsb_dialog == "undefined"){
            gsb_dialog = new Array();
            gsb_dialog_object = new Object();
        }

        //为此对话框设置层级
        this.setFrameZindex = function(){
            var newzindex = me.getNewZindex();
            _framediv.style.zIndex = newzindex;
            _layerdiv.style.zIndex = newzindex - 1;
        }

        //获得最新的层级（防止多个对话框，后面的对话框被前面的覆盖了）
        this.getNewZindex = function(){
            var newzindex = 100;
            for(var i = 0; i < gsb_dialog.length; i++){
                var _zindex = parseInt($(gsb_dialog.length).css("z-index"));
                if (newzindex < _zindex) {
                    newzindex = _zindex;
                }
            }
            newzindex = newzindex + 2;//加2是为了给此对话框的遮罩层留个点
            return newzindex;
        }

        var me = this;
        var isfistshow = true;
        var status = "hide";//默认是隐藏状态
        var meconfig = {
            title: "对话框",
            width: 100,//对话框的宽度
            height: 50,//对话框内容的高度 不包括标题及其它的高度
            animate: true, //是否动态显示或隐藏
            speed: 200, //动显示的速度
            mode: true, //是否模式窗口
            loading:{
                width: 32,
                height: 32
            }
        };

        //把传入的配置参数覆盖默认配置参数
        meconfig = $.extend(true, meconfig, config);
        var _processdiv;//loading frame;

        //对话框 外框
        var _framediv = document.createElement("div");
        _framediv.className = "gsb-dialog";
        _framediv.setAttribute('data-id', Math.random())
        _framediv.style.width = meconfig.width + "px";
        var meposition = Greensoybean.computeMiddlePosition(meconfig.width);
        _framediv.style.left = meposition.left + "px";
        _framediv.style.top = meposition.top + "px";

        //对话框 标题
        var _headerdiv = document.createElement("div");
        _headerdiv.className = "gsb-dialog-header";
        //对话框 标题 内容
        var _headertext = document.createElement("span");
        _headertext.className = "gsb-dialog-header-text";
        _headertext.innerText = meconfig.title;
        _headerdiv.appendChild(_headertext);
        //对话框 标题 关闭按钮
        var _headerclose = document.createElement("span");
        _headerclose.className = "gsb-dialog-header-close";
        _headerclose.innerText = "×";
        _headerdiv.appendChild(_headerclose);
        _headerclose.onclick = function (){
            me.hide();
        }
        //防止事件冒泡
        _headerclose.onmousedown = Greensoybean.stopBubble;
        _headerclose.onmouseup = Greensoybean.stopBubble;
        //对话框 标题 最小化按钮
        //如果是模式窗口，则不能有最小化按钮
        if (meconfig.mode === false) {
            var _headermin = document.createElement("span");
            _headermin.className = "gsb-dialog-header-mix";
            _headermin.innerText = "－";
            _headerdiv.appendChild(_headermin);
            _headermin.onclick = function (){
                me.mix();
            }
            //防止事件冒泡
            _headermin.onmousedown = Greensoybean.stopBubble;
            _headermin.onmouseup = Greensoybean.stopBubble;
        }
        //对话框 标题 还原按钮
        var _headerrecover = document.createElement("span");
        _headerrecover.className = "gsb-dialog-header-recover";
        _headerrecover.innerText = "□";
        _headerdiv.appendChild(_headerrecover);
        _headerrecover.onclick = function(){
            me.show();
        }
        //防止事件冒泡
        _headerrecover.onmousedown = Greensoybean.stopBubble;
        _headerrecover.onmouseup = Greensoybean.stopBubble;

        //对话框 内容
        var _bodydiv = document.createElement("div");
        _bodydiv.className = "gsb-dialog-body";
        _bodydiv.style.height = meconfig.height + "px";

        //遮罩层
        var _layerdiv = document.createElement("div");
        _layerdiv.className = "gsb-layer gsb-opacity-1 gsb-hidden";

        _framediv.appendChild(_headerdiv);
        _framediv.appendChild(_bodydiv);
        //如果显示模式窗口，则显示遮罩层
        if (meconfig.mode) {
            document.body.appendChild(_layerdiv);
        }
        document.body.appendChild(_framediv);
        me.setFrameZindex();
        gsb_dialog.push(_framediv);
        gsb_dialog_object["gsb-dialog-object-" + meconfig.contentid] = me;

        //被拖动事件
        Greensoybean.initDrag(_headerdiv, _framediv);

        //计算显示内容的高度及配置的高度，如果配置高度小，则把显示内容的高度赋值给配置的高度
        var _contentheight = $("#" + meconfig.contentid).height();
        _contentheight = _contentheight + 50;
        meconfig.width = meconfig.width < _contentheight ? _contentheight : meconfig.width;

        //把用户的内容嵌入到对话框内容框中
        var _oldcontent = document.getElementById(meconfig.contentid);
        _bodydiv.appendChild(_oldcontent);
        _oldcontent.setAttribute("gsb-init-dialog", true);

        //窗口变化后，重新计算对话框的位置
        $(window).resize(function(){
            me.computeMixDialogFrame();
        });

        //记录原始高度、宽度、LEFT、TOP
        var oSize = {
            width: $(_framediv).width(),
            height: $(_framediv).height(),
            left: meposition.left,
            top: meposition.top
        };

        //设置对话框的状态
        this.setFrameStatus = function(_status){
            me.status = _status;
            _framediv.setAttribute("data-status", me.status);
            if(_status == "mix"){
                //把最小化的对话框添加到最小化对话框数组中
                gsb_mix_dialog.push(_framediv);
            }
            else{
                //从对话框数组中移除不在最小化状态的对话框
                for(var i = 0;i < gsb_mix_dialog.length; i++){
                    if(gsb_mix_dialog[i].getAttribute("data-id") == _framediv.getAttribute("data-id")){
                        gsb_mix_dialog.splice(i, 1);
                        break;
                    }
                }
            }
        }

        //获得隐藏后的位置信息
        this.getHidePosition = function(){
            var hideWidth = oSize.width > oSize.height ? (oSize.width / oSize.height) : 0;
            var hideHeight = oSize.width < oSize.height ? (oSize.height / oSize.width) : 0;
            var hideTop= $(window).height() - (30 + 45);
            return {
                hideWidth : hideWidth,
                hideHeight : hideHeight,
                hideTop : hideTop
            };
        }

        //对话框的高度、宽度、LEFT、TOP
        this.getOldRangect = function(){
            return {
                width: $(_framediv).width(),
                height: $(_framediv).height(),
                left: $(_framediv).offset().left,
                top: $(_framediv).offset().top
            };
        }

        //计算所有最小化的对话框的位置
        this.computeMixDialogFrame = function(){
            var winWidth = $(window).width();
            var iPreAmount = Math.floor(winWidth / 125);//一行可以放多少个最小化的对话框
            var hideleft = 30;
            var hideposition = me.getHidePosition();
            var hidetop = hideposition.hideTop;
            if(iPreAmount == 0) {
                iPreAmount = 1;
                hideleft = 0;
            }
            var iRows = Math.floor(gsb_mix_dialog.length / iPreAmount);//共多少行
            var _colIndex = 0;//列索引
            for(var i = 0; i < gsb_mix_dialog.length; i++){
                if((i > 0) && (i % iPreAmount == 0)){
                    iRows--;
                    _colIndex = 0;
                }
                $(gsb_mix_dialog[i]).animate({
                    "height" : 30,
                    "width" : 120,
                    "left" : hideleft + _colIndex * 125,
                    "top" : hidetop - iRows * 35
                },
                meconfig.speed);
                _colIndex++;
            }
        }

        //显示
        this.show = function(callback) {
            //如果是动画显示并且第一次显示，要重新设置位置及宽度、高度
            if (meconfig.animate && isfistshow){
                var hideposition = me.getHidePosition();
                $(_framediv).css({
                    left : 30,
                    top : hideposition.hideTop,
                    width : hideposition.hideWidth,
                    height : hideposition.hideHeight
                });
                isfistshow = false;
            }
            _framediv.style.display = "block";
            if (meconfig.mode === false) {
                _headermin.style.display = "block";
            }
            _headerrecover.style.display = "none";
            $(_headerdiv).removeClass("gsb-dialog-header-mix");
            if (meconfig.animate) {
                $(_framediv).animate({
                    "height" : oSize.height,
                    "width" : oSize.width,
                    "left" : oSize.left,
                    "top" : oSize.top
                },meconfig.speed,function(){
                    if(typeof callback == "function"){
                        callback();
                    }
                });
            }
            if (meconfig.mode) {
                _layerdiv.style.display = "block";
                if(typeof callback == "function"){
                    callback();
                }
            }
            me.setFrameStatus("show");
            me.computeMixDialogFrame();
        }

        //隐藏
        this.hide = function() {
            //向内缩小关闭
            if (meconfig.animate) {
                //如果是在显示状态，缩小前记录原始高度及宽度
                if(status == 'show'){
                    oSize = me.getOldRangect();
                }

                var hideposition = me.getHidePosition();
                $(_framediv).animate({
                    "height" : hideposition.hideHeight,
                    "width" : hideposition.hideWidth,
                    "left" : 30,
                    "top" : hideposition.hideTop
                },
                meconfig.speed,
                function(){
                    _framediv.style.display = "none";
                });
            }
            else{
                _framediv.style.display = "none";
            }
            if (meconfig.mode) {
                _layerdiv.style.display = "none";
            }
            me.setFrameStatus("hide");
            me.computeMixDialogFrame();
        }

        this.mix = function(){
            //最小化
            if (meconfig.animate) {
                //最小化前记录原始高度及宽度
                oSize = me.getOldRangect();
                var hideposition = me.getHidePosition();

                //计算最小化后显示的位置
                var icount = $(".gsb-dialog[data-status='mix']").size();//已最小化的数量
                var winWidth = $(window).width();
                var iPreAmount = Math.floor(winWidth / 125);//一行可以放多少个最小化的对话框
                var hideleft = 30;
                var ialeave_amount = 0;//默认最小化后横向放入位置的索引
                if(icount > 0 && (icount % iPreAmount == 0)){
                    $(".gsb-dialog[data-status='mix']").each(function(){
                        $(this).animate({
                            top: $(this).offset().top - 35
                        });
                    });
                }
                else{
                    ialeave_amount = icount % iPreAmount;
                }
                hideleft = hideleft + ialeave_amount * 125;
                $(_framediv).animate({
                    "height" : 30,
                    "width" : 120,
                    "left" : hideleft,
                    "top" : hideposition.hideTop
                },
                meconfig.speed,
                function(){
                    if (meconfig.mode === false) {
                        _headermin.style.display = "none";
                    }
                    _headerrecover.style.display = "block";
                    $(_headerdiv).addClass("gsb-dialog-header-mix");
                });
            }
            else{
                $(_framediv).css({
                    "height" : 30,
                    "width" : 120,
                    "left" : 30,
                    "top" : hideposition.hideTop
                });
                if (meconfig.mode === false) {
                    _headermin.style.display = "none";
                }
                _headerrecover.style.display = "block";
                $(_headerdiv).addClass("gsb-dialog-header-mix");
            }
            me.setFrameStatus("mix");
        }

        this.showProcess = function(){
            _processdiv = document.createElement("div");
            _processdiv.className = "gsb-process gsb-opacity-5";
            _processdiv.style.top = ($(_bodydiv).height() - meconfig.loading.height) / 2 + "px";
            _processdiv.style.left = (meconfig.width - meconfig.loading.width) / 2 + "px";
            _bodydiv.appendChild(_processdiv);
        }

        this.hideProcess = function(){
            _bodydiv.removeChild(_processdiv)
        }
    }
})(jQuery);