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
                'danger': 'rgb(var(--danger) / <alpha-value>)',
                'light': 'rgb(var(--light) / <alpha-value>)',
            },
        },
    },
    plugins: [],
};