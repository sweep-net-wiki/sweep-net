var elements = document.getElementsByTagName('td');

var stat = new Array(); var tmp;
function onmove(e) {
    var tmp = document.getElementById("myid");
    var id = 0;
    var tmp = null;
    if (e.target == null) {
        tmp = e.srcElement;
        id = e.srcElement.getAttribute("myindex");
    } else {
        tmp = e.target;
        id = e.target.getAttribute("myindex");
    }

    id = tmp.getAttribute("myindex");

    if (id == null) {
        tmp = tmp.parentElement;
        id = tmp.getAttribute("myindex");
    }

    if (id == null) {
        tmp = tmp.parentElement;
        id = tmp.getAttribute("myindex");
    }
    if (id == null) return;
    var element = document.getElementById("button_edit_" + id);

    if (element != null && stat[id] == 0) {
        element.style.visibility = 'visible'
    }
    stat[id] = 2;
}

document.body.onload = function () {
    for (var i = 0; i < elements.length; i++) {
        tmp = elements[i].getAttribute("myindex");
        if (tmp != "" && tmp != null) {
            stat[tmp] = 0;
            elements[i].innerHTML = '<a href="action:edit" class="editbutton" id="button_edit_' + tmp + '"><img class="editimg"></a>' + elements[i].innerHTML;
            elements[i].onmousemove = onmove; ;
            elements[i].onclick = onmove;

            var abcd = document.getElementById("button_edit_" + tmp);
            if (abcd != null) abcd.style.visibility = 'hidden'
        }
    }
}

var timer = setInterval(
function () {
    for (var key in stat) {
        if (key === 'length' || !stat.hasOwnProperty(key)) continue;
        var value = stat[key];

        if (value > 0) stat[key]--;
        else {
            var element = document.getElementById("button_edit_" + key);
            if (element != null) element.style.visibility = 'hidden'
        }
    }

},
1000);