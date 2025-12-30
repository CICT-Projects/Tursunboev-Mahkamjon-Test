# 📡 Примеры API запросов

## 🚗 Автомобили (Cars)

### 1️⃣ Добавить новый автомобиль

```bash
curl -X POST http://localhost:5000/api/cars \
  -H "Content-Type: application/json" \
  -d '{
    "name": "BMW X5",
    "brand": "BMW",
    "year": 2023,
    "price": 5000000
  }'
```

**Response (200 OK):**
```json
{
  "id": 1,
  "name": "BMW X5",
  "brand": "BMW",
  "year": 2023,
  "price": 5000000,
  "createdAt": "2024-12-29T10:30:00Z",
  "parts": []
}
```

### 2️⃣ Получить все автомобили

```bash
curl http://localhost:5000/api/cars
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "name": "BMW X5",
    "brand": "BMW",
    "year": 2023,
    "price": 5000000,
    "createdAt": "2024-12-29T10:30:00Z",
    "parts": [
      {
        "id": 1,
        "name": "Масло моторное",
        "price": 5000,
        "type": "двигатель",
        "carId": 1,
        "createdAt": "2024-12-29T10:35:00Z"
      }
    ]
  }
]
```

### 3️⃣ Получить конкретный автомобиль

```bash
curl http://localhost:5000/api/cars/1
```

**Response (200 OK):**
```json
{
  "id": 1,
  "name": "BMW X5",
  "brand": "BMW",
  "year": 2023,
  "price": 5000000,
  "createdAt": "2024-12-29T10:30:00Z",
  "parts": []
}
```

### 4️⃣ Обновить автомобиль

```bash
curl -X PUT http://localhost:5000/api/cars/1 \
  -H "Content-Type: application/json" \
  -d '{
    "id": 1,
    "name": "BMW X7",
    "brand": "BMW",
    "year": 2024,
    "price": 6000000,
    "createdAt": "2024-12-29T10:30:00Z",
    "parts": []
  }'
```

**Response (204 No Content)**

### 5️⃣ Удалить автомобиль

```bash
curl -X DELETE http://localhost:5000/api/cars/1
```

**Response (204 No Content)**

---

## 🔧 Запчасти (Parts)

### 1️⃣ Добавить запчасть к автомобилю

```bash
curl -X POST http://localhost:5000/api/parts \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Масло моторное",
    "price": 5000,
    "type": "двигатель",
    "carId": 1
  }'
```

**Response (200 OK):**
```json
{
  "id": 1,
  "name": "Масло моторное",
  "price": 5000,
  "type": "двигатель",
  "carId": 1,
  "createdAt": "2024-12-29T10:35:00Z"
}
```

### 2️⃣ Получить все запчасти

```bash
curl http://localhost:5000/api/parts
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Масло моторное",
    "price": 5000,
    "type": "двигатель",
    "carId": 1,
    "createdAt": "2024-12-29T10:35:00Z"
  },
  {
    "id": 2,
    "name": "Фильтр воздушный",
    "price": 3500,
    "type": "двигатель",
    "carId": 1,
    "createdAt": "2024-12-29T10:36:00Z"
  }
]
```

### 3️⃣ Получить запчасти конкретного автомобиля

```bash
curl http://localhost:5000/api/parts/car/1
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Масло моторное",
    "price": 5000,
    "type": "двигатель",
    "carId": 1,
    "createdAt": "2024-12-29T10:35:00Z"
  },
  {
    "id": 2,
    "name": "Фильтр воздушный",
    "price": 3500,
    "type": "двигатель",
    "carId": 1,
    "createdAt": "2024-12-29T10:36:00Z"
  }
]
```

### 4️⃣ Обновить запчасть

```bash
curl -X PUT http://localhost:5000/api/parts/1 \
  -H "Content-Type: application/json" \
  -d '{
    "id": 1,
    "name": "Масло моторное синтетическое",
    "price": 7000,
    "type": "двигатель",
    "carId": 1,
    "createdAt": "2024-12-29T10:35:00Z"
  }'
```

**Response (204 No Content)**

### 5️⃣ Удалить запчасть

```bash
curl -X DELETE http://localhost:5000/api/parts/1
```

**Response (204 No Content)**

---

## 🧪 Полный сценарий тестирования

```bash
# 1. Добавить первый автомобиль
curl -X POST http://localhost:5000/api/cars \
  -H "Content-Type: application/json" \
  -d '{"name":"Toyota Camry","brand":"Toyota","year":2023,"price":3000000}'

# 2. Добавить второй автомобиль
curl -X POST http://localhost:5000/api/cars \
  -H "Content-Type: application/json" \
  -d '{"name":"Mercedes-Benz C-Class","brand":"Mercedes","year":2024,"price":4500000}'

# 3. Получить все автомобили
curl http://localhost:5000/api/cars

# 4. Добавить запчасть к первому автомобилю
curl -X POST http://localhost:5000/api/parts \
  -H "Content-Type: application/json" \
  -d '{"name":"Фильтр масла","price":2500,"type":"двигатель","carId":1}'

# 5. Добавить ещё одну запчасть
curl -X POST http://localhost:5000/api/parts \
  -H "Content-Type: application/json" \
  -d '{"name":"Свечи зажигания","price":4000,"type":"двигатель","carId":1}'

# 6. Добавить запчасть ко второму автомобилю
curl -X POST http://localhost:5000/api/parts \
  -H "Content-Type: application/json" \
  -d '{"name":"Тормозные колодки","price":8000,"type":"тормоза","carId":2}'

# 7. Получить запчасти первого автомобиля
curl http://localhost:5000/api/parts/car/1

# 8. Получить все запчасти
curl http://localhost:5000/api/parts

# 9. Получить конкретный автомобиль с запчастями
curl http://localhost:5000/api/cars/1
```

---

## 🔒 Коды ответов

| Code | Значение | Пример |
|------|----------|--------|
| 200 | OK | GET запрос успешен |
| 201 | Created | Ресурс создан (POST) |
| 204 | No Content | Операция успешна, но без тела ответа (PUT/DELETE) |
| 400 | Bad Request | Ошибка в запросе (неправильные данные) |
| 404 | Not Found | Ресурс не найден |
| 500 | Server Error | Ошибка сервера |

---

## 📝 Рекомендации

- Используйте **Postman** или **Insomnia** для удобного тестирования API
- Проверяйте заголовки: `Content-Type: application/json`
- Убедитесь, что Backend запущен перед отправкой запросов
- Используйте реальные ID при обновлении и удалении

---

**Happy Testing! 🎉**
