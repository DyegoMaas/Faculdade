module.exports = function (grunt) {

    var path = require('path');

    // Carregando os plugins
    // grunt.loadNpmTasks('grunt-contrib-compass');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-connect');
    // grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-watch');
    // grunt.loadNpmTasks('grunt-contrib-copy');

    var arquivosJS = {
        app: [
            'app/app.js',
            'app/app.modulos.js',
            'app/app.componentes.js'
        ],
        modulos: [
            'app/modulos/*/src/*.js',
            'app/modulos/*/diretivas/*/*.js',
            'app/modulos/*/modulo.config.js'
        ],
        componentes: [
            'app/componentes/*/diretivas/*/*.js',
            'app/componentes/*/src/*.js',
            'app/componentes/*/modulo.config.js'
        ]
    };

    // Configurando as tarefas
    grunt.initConfig({

        // Compilar SASS usando Compass
        // compass: {
        //     dist: {
        //         options: {
        //             config: 'config.rb'
        //         }
        //     }
        // },

        // Observando alterações
        watch: {
            options: {
                livereload: true
            },
            js: {
                files: arquivosJS.modulos.concat(
                    arquivosJS.componentes,
                    arquivosJS.app
                ),
                tasks: ['concat']
            },
            // sass: {
            //     files: [
            //         'public/sass/*.scss',
            //         'public/sass/*/*.scss'
            //     ],
            //     tasks: ['compass']
            // },
            html: {
                files: [
                    '*.html',
                    'app/modulos/*/view/*.html',
                    'app/modulos/*/view/partials/*.html',
                    'app/modulos/*/diretivas/*/partials/*.html',
                    'app/modulos/*/diretivas/*/partial/*.html',
                    'app/componentes/*/diretivas/*/partials/*.html'
                ]
            }
        },

        // Concatenar
        concat: {
            task : {
                options: {
                    separator: '\n\n'
                },
                src: arquivosJS.modulos.concat(
                    arquivosJS.componentes,
                    arquivosJS.app
                ),
                dest: 'concatenado.js'
            }
        },

        // Minificar
        // uglify: {
        //     task: {
        //         options : {
        //             report : 'gzip',
        //             compress : true,
        //             mangle : true
        //         },
        //         files : {
        //             'public/main.min.js' : ['public/concatenated.js']
        //         }
        //     }
        // },

        // Plugin para o livereload
        connect: {
            server: {
                options: {
                    port: 9000,
                    base: {
                        path: '.',
                        options: {
                            index: 'index.html'
                        }
                    },
                    hostname: "0.0.0.0",
                    livereload: true,
                    open: {
                        target: 'http://localhost:9000/'
                    }
                }
            }
        }
    });

    grunt.registerTask('default', ['concat', 'connect', 'watch']);
};