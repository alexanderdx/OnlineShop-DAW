(function($) {
    $('#carousel-1').carousel({ interval: 3000 })
    const MD_BREAKPOINT_BOOTSTRAP = 768
    if (window.innerWidth >= MD_BREAKPOINT_BOOTSTRAP) {
        const categoryList = $('#category-list')

        const MARGIN_TO_TOP = window.innerWidth * 0.02
        const INITIAL_LIST_HEIGHT = categoryList.offset().top
    
        categoryList.css('position', 'relative');
    
        $(window).on('scroll', ev => {
            if (window.innerWidth < MD_BREAKPOINT_BOOTSTRAP) {
                categoryList.css('position', '')
                return
            }
            const scrollTop = $(window).scrollTop();

            categoryList.stop(false, false).animate({
                top: scrollTop < INITIAL_LIST_HEIGHT
                        ? 0
                        : scrollTop - INITIAL_LIST_HEIGHT + MARGIN_TO_TOP
            }, 250);
        });
    }
})(jQuery);
