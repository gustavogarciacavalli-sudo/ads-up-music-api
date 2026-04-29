const container = document.createElement('div');
container.id = 'mouse-echo-container';
document.body.appendChild(container);

const colors = ['red', 'orange', 'yellow', 'green', 'blue', 'violet'];
let colorIndex = 0;
document.addEventListener('mousemove', (e) => {
    const clone = document.createElement('div');
    clone.className = 'mouse-clone';
    clone.style.left = e.clientX + 'px'; 
    clone.style.top = e.clientY + 'px';
    const currentColor = colors[colorIndex];
    colorIndex = (colorIndex + 1) % colors.length;
    clone.innerHTML = `<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24'><path d='M3 3l7.07 16.97 2.51-7.39 7.39-2.51L3 3z' fill='${currentColor}' stroke='black' stroke-width='1'/></svg>`;
    container.appendChild(clone);
    setTimeout(() => clone.remove(), 1000);
});
