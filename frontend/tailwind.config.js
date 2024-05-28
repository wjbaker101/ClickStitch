/** @type {import('tailwindcss').Config} */
module.exports = {
    corePlugins: {
        preflight: false,
    },
    content: [
        './index.html',
        './src/**/*.{vue,ts}',
    ],
    theme: {
        extend: {
            colors: {
                'primary': 'rgb(var(--primary) / <alpha-value>)',
                'secondary': 'rgb(var(--secondary) / <alpha-value>)',

                'danger': 'rgb(var(--danger) / <alpha-value>)',

                'light': 'rgb(var(--light) / <alpha-value>)',
                'dark': 'rgb(var(--dark) / <alpha-value>)',

                'background': 'rgb(var(--background) / <alpha-value>)',
            },
        },
    },
    plugins: [],
};