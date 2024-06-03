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

                'warning': 'rgb(var(--warning) / <alpha-value>)',

                'light': 'rgb(var(--light) / <alpha-value>)',
                'dark': 'rgb(var(--dark) / <alpha-value>)',

                'background': 'rgb(var(--background) / <alpha-value>)',
                'background-dark': 'rgb(var(--background-dark) / <alpha-value>)',
                'background-light': 'rgb(var(--background-light) / <alpha-value>)',
            },

            gridTemplateColumns: {
                'for-patterns': 'repeat(auto-fill, minmax(min(350px, 100%), 1fr))',
            },
            borderWidth: {
                '1': '1px',
            },

            backgroundImage: {
                'texture': 'var(--background-texture)',
                'image-inherit': 'inherit',
            },

            keyframes: {
                'loading': {
                    '0%': { color: 'rgb(var(--primary))' },
                    '50%': { color: 'rgb(var(--secondary))' },
                    '100%': { color: 'rgb(var(--primary))' },
                },
            },

            animation: {
                'loading': 'loading 3s ease-in-out infinite',
            },

            dropShadow: {
                'icon': '1px 1px rgba(0, 0, 0, 0.6)',
            },

            textColor: {
                'colour': 'var(--text-colour)',
            },

            fontFamily: {
                'mulish': ['Mulish', 'ui-sans-serif', 'system-ui', 'sans-serif', '"Apple Color Emoji"', '"Segoe UI Emoji"', '"Segoe UI Symbol"', '"Noto Color Emoji"'],
            },
        },
    },
    plugins: [],
};