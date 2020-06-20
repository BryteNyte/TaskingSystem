(function ($) {
    function HomeIndex() {
        var $this = this;

        function initialize() {

            $('#Description').summernote({
                focus: true,
                height: 150,
                codemirror: {
                    theme: 'united'
                }

            });
            $('.dropdown-toggle').dropdown();
        }

        $this.init = function () {
            initialize();
        };
    }
    $(function () {
        var self = new HomeIndex();
        self.init();
    });
}(jQuery)); 

