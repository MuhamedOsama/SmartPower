/* chartist-plugin-pointlabels 0.0.12
 * Copyright © 2016 Gion Kunz
 * Free to use under the WTFPL license.
 * http://www.wtfpl.net/
 */

!function(a,b){"function"==typeof define&&define.amd?define(["chartist"],function(c){return a.returnExportsGlobal=b(c)}):"object"==typeof exports?module.exports=b(require("chartist")):a["Chartist.plugins.tooltips"]=b(Chartist)}(this,function(a){return function(a,b,c){"use strict";function d(a){f(a,"tooltip-show")||(a.className=a.className+" tooltip-show")}function e(a){var b=new RegExp("tooltip-show\\s*","gi");a.className=a.className.replace(b,"").trim()}function f(a,b){return(" "+a.getAttribute("class")+" ").indexOf(" "+b+" ")>-1}function g(a,b){do a=a.nextSibling;while(a&&!f(a,b));return a}function h(a){return a.innerText||a.textContent}var i={currency:void 0,tooltipOffset:{x:0,y:-20},appendToBody:!1,"class":void 0,pointClass:"ct-point"};c.plugins=c.plugins||{},c.plugins.tooltip=function(a){return a=c.extend({},i,a),function(i){function j(a,b,c){m.addEventListener(a,function(a){(!b||f(a.target,b))&&c(a)})}function k(b){o=o||n.offsetHeight,p=p||n.offsetWidth,a.appendToBody?(n.style.top=b.pageY-o+a.tooltipOffset.y+"px",n.style.left=b.pageX-p/2+a.tooltipOffset.x+"px"):(n.style.top=(b.layerY||b.offsetY)-o+a.tooltipOffset.y+"px",n.style.left=(b.layerX||b.offsetX)-p/2+a.tooltipOffset.x+"px")}var l=a.pointClass;i instanceof c.Bar?l="ct-bar":i instanceof c.Pie&&(l=i.options.donut?"ct-slice-donut":"ct-slice-pie");var m=i.container,n=m.querySelector(".chartist-tooltip");n||(n=b.createElement("div"),n.className=a["class"]?"chartist-tooltip "+a["class"]:"chartist-tooltip",a.appendToBody?b.body.appendChild(n):m.appendChild(n));var o=n.offsetHeight,p=n.offsetWidth;e(n),j("mouseover",l,function(e){var f=e.target,j="",l=i instanceof c.Pie?f:f.parentNode,m=l?f.parentNode.getAttribute("ct:meta")||f.parentNode.getAttribute("ct:series-name"):"",q=f.getAttribute("ct:meta")||m||"",r=!!q,s=f.getAttribute("ct:value");if(a.tooltipFnc)j=a.tooltipFnc(q,s);else{if(a.metaIsHTML){var t=b.createElement("textarea");t.innerHTML=q,q=t.value}if(q='<span class="chartist-tooltip-meta">'+q+"</span>",r)j+=q+"<br>";else if(i instanceof c.Pie){var u=g(f,"ct-label");u&&(j+=h(u)+"<br>")}s&&(a.currency&&(s=a.currency+s.replace(/(\d)(?=(\d{3})+(?:\.\d+)?$)/g,"$1,")),s='<span class="chartist-tooltip-value">'+s+"</span>",j+=s)}j&&(n.innerHTML=j,k(e),d(n),o=n.offsetHeight,p=n.offsetWidth)}),j("mouseout",l,function(){e(n)}),j("mousemove",null,function(a){k(a)})}}}(window,document,a),a.plugins.tooltips});
//# sourceMappingURL=chartist-plugin-tooltip.min.js.map