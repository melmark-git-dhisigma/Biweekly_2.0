


        $(function() {
            function launch() {
                 $('#sign_up').lightbox_me({centered: true, onLoad: function() { $('#sign_up').find('input:first').focus()}});
            }
            
            $('#try-1').click(function(e) {
                $("#sign_up").lightbox_me({centered: true, onLoad: function() {
					$("#sign_up").find("input:first").focus();
				}});
				
                e.preventDefault();
            });
            
            
            $('table tr:nth-child(even)').addClass('stripe');
        });
