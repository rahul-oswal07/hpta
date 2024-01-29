/** Get current viewport height in pixels. */
export function getWindowHeight() {
  return window.innerHeight ||
    document.documentElement.clientHeight ||
    document.body.clientHeight || 0;
}
