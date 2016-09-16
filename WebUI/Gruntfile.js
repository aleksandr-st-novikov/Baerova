﻿/// <binding BeforeBuild='all' />
/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {
    grunt.initConfig({
        clean: ["lib/*", "temp/*"],
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
                        'temp/site.js': ['Scripts/bootstrap.js', 'Scripts/respond.js', 'Scripts/site.js'],
                        'temp/jquery.js': ['Scripts/jquery-?????.js', 'Scripts/jquery.validate.js', 'Scripts/jquery.validate.unobtrusive.js',
                            'Scripts/modernizr-?????.js', 'Scripts/jquery.unobtrusive-ajax.js'],
                        'lib/js/kendo.min.js': ['Scripts/kendo/jquery.min.js', 'Scripts/kendo/kendo.all.min.js', 'Scripts/kendo/kendo.aspnetmvc.min.js']
                    }
            },
            css: {
                files:
                    {
                        'temp/site.css': ['Content/bootstrap.css', 'Content/font-awesome.css', 'Content/site.css', 'Content/sidemenu.css'],
                        'lib/css/kendo.min.css': ['Content/kendo/kendo.common-nova.min.css', 'Content/kendo/kendo.nova.min.css']
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
            js: {
                files:
                    {
                        'lib/js/site.min.js': ['temp/site.js'],
                        'lib/js/jquery.min.js': ['temp/jquery.js']
                    }
            }
        },
        //watch: {
        //    files: ["Scripts/*.js"],
        //    tasks: ["all"]
        //},
        cssmin: {
            css: {
                files:
                    {
                        'lib/css/site.min.css': 'temp/site.css'
                    }
            }
        },
        //imagemin: {
        //    all: {
        //        files: [{
        //            expand: true,
        //            cwd: 'catalog',
        //            src: ['*.{png,jpg,gif}'],
        //            dest: 'catalog1'
        //        }]
        //    }
        //},
        copy:
            {
                fonts: {
                    expand: true,
                    src: 'fonts/*',
                    dest: 'lib'
                },
                nova: {
                    expand: true,
                    cwd: 'content/kendo/nova',
                    src: '**',
                    dest: 'lib/css/nova/'
                }
            }
    });

    grunt.loadNpmTasks("grunt-contrib-clean");
    //grunt.loadNpmTasks('grunt-contrib-jshint');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-contrib-copy');
    //grunt.loadNpmTasks('grunt-contrib-imagemin');
    //grunt.loadNpmTasks('grunt-contrib-watch');

    //grunt.registerTask("all", ['clean', 'concat', 'uglify', 'cssmin', 'imagemin']);
    grunt.registerTask("all", ['clean', 'concat', 'uglify', 'cssmin', 'copy']);
};