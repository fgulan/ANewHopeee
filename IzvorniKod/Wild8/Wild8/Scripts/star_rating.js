/*
   ___   _  __           __ __
  / _ | / |/ /__ _    __/ // /__  ___  ___ ___ ___
 / __ |/    / -_) |/|/ / _  / _ \/ _ \/ -_) -_) -_)
/_/ |_/_/|_/\__/|__,__/_//_/\___/ .__/\__/\__/\__/
                               /_/
*/

var __slice = [].slice;
(function($, window) {
    var Stars;
    Stars = (function() {
        Stars.prototype.defaults = {
            rating: void 0,
            numStars: 5,
            change: function(e, value) {}
        };

        function Stars($el, options) {
            var i, _, _ref,
                _this = this;
            this.options = $.extend({}, this.defaults, options);
            this.$el = $el;
            _ref = this.defaults;
            for (i in _ref) {
                _ = _ref[i];
                if (this.$el.data(i) != null) {
                    this.options[i] = this.$el.data(i);
                }
            }
            this.createStars();
            this.syncRating();
            this.$el.on('mouseover.stars', 'span', function(e) {
                return _this.syncRating(_this.$el.find('span').index(e.currentTarget) + 1);
            });
            this.$el.on('mouseout.stars', function() {
                return _this.syncRating();
            });
            this.$el.on('click.stars', 'span', function(e) {
                return _this.setRating(_this.$el.find('span').index(e.currentTarget) + 1);
            });
            this.$el.on('stars:change', this.options.change);
        }
        Stars.prototype.createStars = function() {
            var _i, _ref, _results;
            _results = [];
            for (_i = 1, _ref = this.options.numStars; 1 <= _ref ? _i <= _ref : _i >= _ref; 1 <= _ref ? _i++ : _i--) {
                _results.push(this.$el.append("<span class='glyphicon .glyphicon-star-empty'></span>"));
            }
            return _results;
        };
        Stars.prototype.setRating = function(rating) {
            if (this.options.rating === rating) {
                rating = void 0;
            }
            this.options.rating = rating;
            this.syncRating();
            return this.$el.trigger('stars:change', rating);
        };
        Stars.prototype.syncRating = function(rating) {
            var i, _i, _j, _ref;
            rating || (rating = this.options.rating);
            if (rating) {
                for (i = _i = 0, _ref = rating - 1; 0 <= _ref ? _i <= _ref : _i >= _ref; i = 0 <= _ref ? ++_i : --_i) {
                    this.$el.find('span').eq(i).removeClass('glyphicon-star-empty').addClass('glyphicon-star');
                }
            }
            if (rating && rating < 5) {
                for (i = _j = rating; rating <= 4 ? _j <= 4 : _j >= 4; i = rating <= 4 ? ++_j : --_j) {
                    this.$el.find('span').eq(i).removeClass('glyphicon-star').addClass('glyphicon-star-empty');
                }
            }
            if (!rating) {
                return this.$el.find('span').removeClass('glyphicon-star').addClass('glyphicon-star-empty');
            }
        };
        return Stars;
    })();
    return $.fn.extend({
        stars: function() {
            var args, option;
            option = arguments[0], args = 2 <= arguments.length ? __slice.call(arguments, 1) : [];
            return this.each(function() {
                var data;
                data = $(this).data('star-rating');
                if (!data) {
                    $(this).data('star-rating', (data = new Stars($(this), option)));
                }
                if (typeof option === 'string') {
                    return data[option].apply(data, args);
                }
            });
        }
    });
})(window.jQuery, window);

$(function() {
    return $('#stars').stars();
});
$(document).ready(function() {
    $('#stars').on('stars:change', function(e, value) {
        $('#count').html(value);
    });
    $('#stars-existing').on('stars:change', function(e, value) {
        $('#count-existing').html(value);
    });
});
