/// <reference path="jquery-1.4.4-vsdoc.js" />
if (typeof (jQuery) == "undefined") {
    alert("未检测到jQuery脚本库，ASP.NET MvcPager无法实现jQuery Ajax分页！");
}
else {
    jQuery.extend({
        bitPager: {
            doPage: function (config) {
                if (config.MsAjax) {
                    return;
                }
                var _mvcPostFun = function () {
                    if (!jQuery(this).attr('href')) {
                        return;
                    }
                    if (this.getAttribute("disabled") == "disabled") {
                        return;
                    }
                    if (this.onclick != null || this.href.indexOf('#') > -1) {
                        return;
                    }
                    if (!config.UpdateTargetId) {
                        window.open(this.attributes.getNamedItem('href').value, '_self'); return false;
                    }

                    $.ajax({
                        type: config.HttpMethod,
                        url: jQuery(this).attr('href'),
                        data: { __r: Math.random() },
                        cache: false,
                        success: function (data, status, xhr) {
                            if (config.AjaxFun.OnSuccess) {
                                config.AjaxFun.OnSuccess(data, status, xhr);
                            }
                            jQuery('#' + config.UpdateTargetId + '').html(data);
                        },
                        error: config.AjaxFun.OnFailure || function () { },
                        beforeSend: config.AjaxFun.OnBegin || function () { },
                        complete: config.AjaxFun.OnComplete || function () { }
                    });
                    return false;
                }
                jQuery('#' + config.Id + ' a').click(_mvcPostFun);
            },
            keyDown: function (e, totalPage, currentPage) {
                var _kc, _pib;
                if (window.event) {
                    _kc = e.keyCode;
                    _pib = e.srcElement;
                }
                else if (e.which) {
                    _kc = e.which;
                    _pib = e.target;
                }
                var validKey = (_kc == 8 || _kc == 46 || _kc == 37 || _kc == 39 || (_kc >= 48 && _kc <= 57) || (_kc >= 96 && _kc <= 105));
                if (!validKey) {
                    if (_kc == 13) {
                        this.goToPage(_pib, totalPage, currentPage); //MaxPage
                    }
                    if (e.preventDefault) {
                        e.preventDefault();
                    }
                    else {
                        event.returnValue = false;
                    }
                }
            },
            goToPage: function (_pib, _mp, _currentPage, _invalidTip, _outRangeTip) {
                var pageIndex;
                if (_pib.tagName == "SELECT") {
                    pageIndex = _pib.options[_pib.selectedIndex].value;
                }
                else {
                    pageIndex = _pib.value;
                    var r = new RegExp("^\\s*(\\d+)\\s*$");
                    if (!r.test(pageIndex)) {
                        alert(_invalidTip || "页索引无效");
                        return;
                    }
                    else if (RegExp.$1 < 1 || RegExp.$1 > _mp) {
                        alert(_outRangeTip || "页索引超出范围");
                        return;
                    }
                }
                if (_currentPage == pageIndex) {
                    return false;
                }
                var _hl = document.getElementById(_pib.id + 'link').childNodes[0];
                var _lh = _hl.href;
                _hl.href = _lh.substring(0, _lh.length - 1) + pageIndex;
                jQuery(_hl).click();
                _hl.href = _lh;
            }
        }
    });
}