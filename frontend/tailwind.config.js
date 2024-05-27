/** @type {import('tailwindcss').Config} */
module.exports = {
    prefix: '',
    content: [
        './index.html',
        './src/**/*.{vue,ts}',
    ],
    theme: {
        extend: {},
        colors: {
            'danger': 'rgb(var(--danger) / <alpha-value>)',
            'light': 'rgb(var(--light) / <alpha-value>)',
        },
    },
    plugins: [],
};