// addEventListener polyfill 1.0 / Eirik Backer / MIT Licence
(function (win, doc) {
    //return;
	if(win.addEventListener)return;		//No need to polyfill

	function docHijack(p){var old = doc[p];doc[p] = function(v){return addListen(old(v))}}
	function addEvent(on, fn, self){
		return (self = this).attachEvent('on' + on, function(e){
			var e = e || win.event;
			e.preventDefault  = e.preventDefault  || function(){e.returnValue = false}
			e.stopPropagation = e.stopPropagation || function(){e.cancelBubble = true}
			fn.call(self, e);
		});
	}
	function addListen(obj, i) {
	    if (obj) {
	        if (i = obj.length) while (i--) obj[i].addEventListener = addEvent;
	        else obj.addEventListener = addEvent;
	    } 
		return obj;
	}

	addListen([doc, win]);
	if('Element' in win)win.Element.prototype.addEventListener = addEvent;			//IE8
	else{																			//IE < 8
		doc.attachEvent('onreadystatechange', function(){addListen(doc.all)});		//Make sure we also init at domReady
		docHijack('getElementsByTagName');
		docHijack('getElementById');
		docHijack('createElement');
		addListen(doc.all);	
	}
})(window, document);

var sfs = {};

window.addEventListener('load', pageLoad, false);

function pageLoad() {
    sfs.preventFraming();
} // END pageLoad

/**
Makes it harder to place the page in an iframe.
*/
sfs.preventFraming = function() {
    // if not in a frame...
    if (self === top) {
        // look for an element called antiClickjack (it will be a <style> element)
        var antiClickjack = document.getElementById("antiClickjack");

        // if found, remove it.
        if (antiClickjack) {
            antiClickjack.parentNode.removeChild(antiClickjack);
        }
    } else {
        // if in a frame, try to bust out of it.
        top.location = self.location;
    }
}; // END preventFraming