/**
 * Applies the given theme
 * @param theme auto, light, dark
 */
function applyTheme(theme : "light" | "dark"){
    document.documentElement.setAttribute('data-bs-theme', theme)
    if(theme == "dark"){
        document.documentElement.classList.add('p-color-mode-dark');
    } else {
        document.documentElement.classList.remove('p-color-mode-dark');
    }
}

export function handleColorMode(){
    applyTheme(window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light');
    window.matchMedia('(prefers-color-scheme: dark)')
        .addEventListener('change',({ matches }) => {
            if (matches) {
                applyTheme('dark')
            }
        })
    window.matchMedia('(prefers-color-scheme: light)')
        .addEventListener('change',({ matches }) => {
            if (matches) {
                applyTheme('light')
            }
        })
}