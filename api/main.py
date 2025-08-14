from fastapi import FastAPI
from pydantic import BaseModel
from typing import List

app = FastAPI()

# Fake student marks data
students_data = [
    {"name": "Alice", "marks": 95},
    {"name": "Bob", "marks": 89},
    {"name": "Charlie", "marks": 92},
    {"name": "David", "marks": 88},
    {"name": "Eve", "marks": 91},
] * 5  # multiply to have more than 20

# Request model for top students
class TopStudentsRequest(BaseModel):
    limit: int

# Response model
class Student(BaseModel):
    name: str
    marks: int

@app.post("/get-top-students", response_model=List[Student])
def get_top_students(req: TopStudentsRequest):
    # Sort by marks descending and get top N
    sorted_students = sorted(students_data, key=lambda x: x["marks"], reverse=True)
    return sorted_students[:req.limit]
