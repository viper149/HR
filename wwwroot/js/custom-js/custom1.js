$(function () {
    const form = $("form");
    const scrollBtn = $("a[href='#top']");

    toastrNotification('@TempData["message"]', '@TempData["type"]');

    // SEARCH IN THE MENUS
    $('input[name="menuSearch"]').keyup(function () {
        const matches = $('ul#myMenu').find(`li:contains(${$(this).val()}) `);
        $('li', 'ul#myMenu').not(matches).slideUp();
        matches.slideDown();
    });

    $('input[type="search"]').on("click", function () {
        $('li', 'ul#myMenu').slideDown();
    });

    $(".bxslider").bxSlider({
        auto: true,
        autoControls: false,
        controls: false,
        pause: 5000,
        infiniteLoop: true,
        responsive: true,
        mode: 'horizontal',
        minSlides: 1,
        autoDelay: 0,
        randomStart: true,
        pager: false,
        moveSlideQty: 1,
        touchEnabled: false
    });

    $('.js-example-basic-single').select2({
        //sorter: function(data) {
        //    /* Sort data using lowercase comparison */

        //    return data.sort(function(a, b) {
        //        a = a.text.toLowerCase();
        //        b = b.text.toLowerCase();

        //        if (a[0] === 's' && b[0] === 's') {
        //            return compare(a, b);
        //        } else if (a[0] === 's') {
        //            return -1;
        //        } else if (b[0] === 's') {
        //            return 1;
        //        } else {
        //            return compare(a, b);
        //        }
        //    });
        //}
    });

    //function compare(a, b) {
    //    if (a > b) {
    //        return 1;
    //    } else if (a < b) {
    //        return -1;
    //    }
    //    return 0;
    //}

    //function matchStart(params, data) {
    //    // If there are no search terms, return all of the data
    //    if ($.trim(params.term) === '') {
    //        return data;
    //    }

    //    // Skip if there is no 'children' property
    //    if (typeof data.children === 'undefined') {
    //        return null;
    //    }

    //    // `data.children` contains the actual options that we are matching against
    //    var filteredChildren = [];
    //    $.each(data.children, function (idx, child) {
    //        if (child.text.toUpperCase().indexOf(params.term.toUpperCase()) == 0) {
    //            filteredChildren.push(child);
    //        }
    //    });

    //    // If we matched any of the timezone group's children, then set the matched children on the group
    //    // and return the group object
    //    if (filteredChildren.length) {
    //        var modifiedData = $.extend({}, data, true);
    //        modifiedData.children = filteredChildren;

    //        // You can return modified objects from here
    //        // This includes matching the `children` how you want in nested data sets
    //        return modifiedData;
    //    }

    //    // Return `null` if the term should not be displayed
    //    return null;
    //}

    form.bind("keypress",
        function (event) {
            if (event.keyCode === 13) {
                return false;
            }
            return true;
        });

    scrollBtn.click(function () {
        $("html, body").animate({ scrollTop: 0 }, "slow");
        return false;
    });

    $(window).scroll(function () {
        $(this).scrollTop() > 50 ? scrollBtn.fadeIn() : scrollBtn.fadeOut();
    });

    $('#menu_toggle').click(function () {
        $(this).find("i").toggleClass("fa fa-bars").toggleClass("fa fa-close rotated");
    });

    var interval = setInterval(function () {
        const momentNow = moment();
        $('#time-part').html(`${momentNow.format('dddd, MMMM Do YYYY, h:mm:ss A')}`);
    }, 100);

    $('#stop-interval').on('click', function () {
        clearInterval(interval);
    });

});


// on first focus (bubbles up to document), open the menu
$(document).on('focus', '.select2-selection.select2-selection--single', function (e) {
    $(this).closest(".select2-container").siblings('select:enabled').select2('open');
});

// steal focus during close - only capture once and stop propogation
$('select.select2').on('select2:closing', function (e) {
    $(e.target).data("select2").$selection.one('focus focusin', function (e) {
        e.stopPropagation();
    });
});

var deleteSweetalert = function () { deleteSweetalert(); }
// For Datatable Column Char limit

if ($.fn.dataTable !== undefined) {
    $.fn.dataTable.render.ellipsis = function (cutoff) {
        return function (data, type, row) {
            return data !== null && type === 'display' && data.length > cutoff ?
                data.substr(0, cutoff) + '…' :
                data;
        }
    };
}