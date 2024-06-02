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
                'primary-dark': 'rgb(var(--primary-dark) / <alpha-value>)',

                'secondary': 'rgb(var(--secondary) / <alpha-value>)',
                'secondary-dark': 'rgb(var(--secondary-dark) / <alpha-value>)',

                'danger': 'rgb(var(--danger) / <alpha-value>)',

                'light': 'rgb(var(--light) / <alpha-value>)',
                'dark': 'rgb(var(--dark) / <alpha-value>)',

                'background': 'rgb(var(--background) / <alpha-value>)',
                'background-dark': 'rgb(var(--background-dark) / <alpha-value>)',
                'background-light': 'rgb(var(--background-light) / <alpha-value>)',
            },

            gridTemplateColumns: {
                'for-patterns': 'repeat(auto-fill, minmax(min(350px, 100%), 1fr))',
            },

            backgroundImage: {
                'texture': 'var(--background-texture)',
            },

            keyframes: {
                loading: {
                    '0%': { color: 'rgb(var(--primary))' },
                    '50%': { color: 'rgb(var(--secondary))' },
                    '100%': { color: 'rgb(var(--primary))' },
                },
            },

            animation: {
                loading: 'loading 3s ease-in-out infinite',
            },
        },
    },
    plugins: [],
};