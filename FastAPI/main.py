from fastapi import FastAPI, UploadFile, File, Form
from fastapi.responses import FileResponse
from pydantic import BaseModel
import shutil

app = FastAPI()

@app.get("/")
def read_root():
    return {"message": "Hello, World!"}

@app.get("/text")
def get_text():
    return {"message": "Hello from the server!"}

@app.post("/text")
def post_text(text: str = Form(...)):
    print(f"Received text: {text}")
    return {"message": f"Received text: {text}"}

@app.get("/texture")
def get_texture():
    return FileResponse("eto_hebi_hat.png")

@app.post("/texture")
async def post_texture(texture: UploadFile = File(...)):
    with open(f"uploaded_{texture.filename}", "wb") as buffer:
        shutil.copyfileobj(texture.file, buffer)
    return {"message": "Texture uploaded successfully"}

class Data(BaseModel):
    id: int
    name: str
    value: float

@app.get("/json")
def get_json():
    data = Data(id=1, name="test", value=3.14)
    return data

@app.post("/json")
def post_json(data: Data):
    print(f"Received data: {data.id}, {data.name}, {data.value}")
    return {"message": f"Received data: {data}"}
