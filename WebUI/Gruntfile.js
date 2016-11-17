/// <binding BeforeBuild='all' />
/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {
    grunt.initConfig({
        clean: {
            css: ["lib/*.css", "temp/*.css"],
            js: ["lib/*.js", "temp/*.js"],
            all: ["lib/*", "temp/*"]
        },
        concat: {
            options: {
                stripBanners: {
                    block: true,
                    line: true
                }
            },
            js: {
                files:
                    {
                        'temp/site.js': ['Scripts/bootstrap.js', 'Scripts/respond.js', 'Scripts/site.js', 'Scripts/jquery.maskedinput.js'],
                        'temp/jquery.js': ['Scripts/jquery-?????.js', 'Scripts/jquery.validate.js', 'Scripts/jquery.validate.unobtrusive.js',
                            'Scripts/modernizr-?????.js', 'Scripts/jquery.unobtrusive-ajax.js', 'Scripts/bootstrap-maxlength.js'],
                        'lib/js/bootstrap-datepicker.min.js': ['Scripts/bootstrap-datepicker.min.js', 'Scripts/locales/bootstrap-datepicker.ru.min.js'],
                        'lib/js/kendo.all.min.js': ['Scripts/kendo/jquery.min.js', 'Scripts/kendo/kendo.all.min.js', 'Scripts/kendo/kendo.aspnetmvc.min.js'],
                        'temp/kendo.js': ['Scripts/jquery.validate.js', 'Scripts/jquery.validate.unobtrusive.js', 'Scripts/bootstrap-maxlength.js']
                    }
            },
            css: {
                files:
                    {
                        'temp/site.css': ['Content/bootstrap.css', 'Content/font-awesome.css', 'Content/sidemenu.css', 'Content/site.css', 'Content/bootstrap-datetimepicker.css'],
                        'lib/css/kendo.min.css': ['Content/kendo/kendo.common-nova.min.css', 'Content/kendo/kendo.nova.min.css'],
                        'lib/css/bootstrap-datepicker.min.css': ['Content/bootstrap-datepicker.min.css']
                    }
            }
        },
        //jshint: {
        //    files: ['temp/*.js'],
        //    options: {
        //        '-W069': false,
        //    }
        //},
        uglify: {
            options: {
                compress: true,
                mangle: true,
                sourceMap: true
            },
            js: {
                files:
                    {
                        'lib/js/site.min.js': ['temp/site.js'],
                        'lib/js/jquery.min.js': ['temp/jquery.js'],
                        'lib/js/kendo.min.js': ['temp/kendo.js']
                    }
            }
        },
        cssmin: {
            css: {
                files:
            {
                'lib/css/site.min.css': 'temp/site.css'
            }
            }
        },
        imagemin: {
            all: {
                files: [{
                    expand: true,
                    cwd: 'content/userfiles/images',
                    src: ['*.{png,jpg,gif}'],
                    dest: 'content/userfiles/images'
                }]
            }
        },
        copy:
            {
                fonts: {
                    expand: true,
                    src: 'fonts/*',
                    dest: 'lib'
                },
                kendoNova: {
                    expand: true,
                    cwd: 'content/kendo/nova',
                    src: '**',
                    dest: 'lib/css/nova/'
                },
                images: {
                    expand: true,
                    flatten: true,
                    src: 'content/userfiles/images/main/header.jpg',
                    dest: 'lib/css/'
                }
            },
        watch: {
            css: {
                files: ['Content/bootstrap.css', 'Content/font-awesome.css', 'Content/site.css', 'Content/sidemenu.css', 'Content/bootstrap-datetimepicker.css',
                        'Content/kendo/kendo.common-nova.min.css', 'Content/kendo/kendo.nova.min.css', 'Content/bootstrap-datepicker.min.css'],
                tasks: ['css']
            },
            js: {
                files: ['Scripts/bootstrap.js', 'Scripts/respond.js', 'Scripts/site.js', 'Scripts/jquery.maskedinput.js',
                        'Scripts/jquery-?????.js', 'Scripts/jquery.validate.js', 'Scripts/jquery.validate.unobtrusive.js',
                        'Scripts/modernizr-?????.js', 'Scripts/jquery.unobtrusive-ajax.js', 'Scripts/bootstrap-maxlength.js',
                        'Scripts/bootstrap-datepicker.min.js', 'Scripts/locales/bootstrap-datepicker.ru.min.js',
                        'Scripts/kendo/jquery.min.js', 'Scripts/kendo/kendo.all.min.js', 'Scripts/kendo/kendo.aspnetmvc.min.js',
                        'Scripts/jquery.validate.js', 'Scripts/jquery.validate.unobtrusive.js', 'Scripts/bootstrap-maxlength.js'],
                tasks: ['js']
            }
        }
    });

    grunt.loadNpmTasks("grunt-contrib-clean");
    //grunt.loadNpmTasks('grunt-contrib-jshint');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-contrib-imagemin');
    grunt.loadNpmTasks('grunt-contrib-watch');

    //grunt.registerTask("all", ['clean', 'concat', 'uglify', 'cssmin', 'imagemin']);
    grunt.registerTask("all", ['clean:all', 'concat', 'uglify', 'cssmin', 'imagemin', 'copy']);
    grunt.registerTask("css", ['clean:css', 'concat', 'cssmin']);
    grunt.registerTask("js", ['clean:js', 'concat', 'uglify']);
};