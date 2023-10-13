const postMessage = async (message) => {
    try {
        await axios.post("/api/message", message);
        location.href = "/photo/indexuser";
    } catch (error) {
        console.error("Error posting the message:", error);
    }
};

const createForm = () => {
    const form = document.querySelector("#message-create-form");

    form.addEventListener("submit", async (e) => {
        e.preventDefault();
        const message = extractMessageFromForm(form);
        await postMessage(message);
    });
};

const extractMessageFromForm = (form) => {
    const email = form.querySelector("#email").value.trim();
    const text = form.querySelector("#text").value.trim();

    return {
        id: 0,
        email,
        text,
    };
};


const loadPhotos = async (filter) => {
    const photos = await getPhotos(filter);
    renderPhotos(photos);
};

const getPhotos = async (title) => {
    try {
        const response = await axios.get('/api/photo', title ? { params: { title } } : {});
        return response.data;
    } catch (error) {
        console.error("Error fetching photos:", error);
        return [];
    }
};

const renderPhotos = (photos) => {
    const emptyMessage = document.querySelector("#emptyMessage");
    const loader = document.querySelector("#loader");
    const photoList = document.querySelector("#photoList");
    const table = document.querySelector("#table");
    const filter = document.querySelector("#filter");

    const hasPhotos = photos && photos.length > 0;

    table.style.display = hasPhotos ? "block" : "none";
    filter.style.display = hasPhotos ? "block" : "none";
    emptyMessage.style.display = hasPhotos ? "none" : "block";
    loader.style.display = "none";

    photoList.innerHTML = photos.map(photoComponent).join('');
};

const initFilter = () => {
    const filterInput = document.querySelector("#filter input");
    filterInput.addEventListener("input", (e) => loadPhotos(e.target.value));
};

const photoComponent = (photo) => `
    <div class="col-12 col-md-6 text-center d-flex flex-column align-items-center py-3 justify-content-center">
        <div class="d-flex flex-column px-2 w-100 h-100">
            <div class="w-100 h-100">
                <a href="/photo/show/${photo.id}">
                    <img class="h-100 w-100" style="object-fit: cover;" src="/${photo.url}" alt="${photo.title}">
                </a>
            </div>
            <h5>${photo.title}</h5>
            <p>${photo.description}</p>
        </div>
    </div>
`;


