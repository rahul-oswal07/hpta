/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: 'class',
  content: [
    './src/**/*.{html,scss,ts}'
  ],
  theme: {
    screens: {
      xs: '480px', // example breakpoint for xs
      sm: '640px', // default small breakpoint
      md: '768px', // default medium breakpoint
      lg: '1024px', // default large breakpoint
      xl: '1280px', // default extra-large breakpoint
      '2xl': '1536px' // an extra breakpoint if needed
      // Add more custom breakpoints if needed
    }
    // other theme configurations
  },
  plugins: []
}
