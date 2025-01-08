const sliderList = document.querySelector(".slider__list");
const right = document.querySelector(".slider__arrow--right");
const left = document.querySelector(".slider__arrow--left");
const sliderItems = document.querySelectorAll(".slider__item");

let activeIndex = 0;

function render() {
  sliderList.style.transform = `translateX(-${88 * activeIndex}vw)`;
  for (const sliderItem of sliderItems) {
    sliderItem.classList.remove("slider__item--active");
  }
  sliderItems[activeIndex].classList.add("slider__item--active");
}

left.onclick = () => {
  if (activeIndex === 0) return;

  activeIndex--;

  render();
};

right.onclick = () => {
  if (activeIndex === 3) return;

  activeIndex++;

  render();
};
