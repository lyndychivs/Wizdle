# 🧩 Wizdle.Api

**Version:** 1.0.0

A simple API to help solve word puzzles by narrowing down possible matches based on input clues.

---

## 🎯 `POST`

**Summary**: Processes a Wizdle request in an attempt to solve the possible words.

### Request

**Content-Type:** `application/json`

#### Body Parameters

| Name               | Type     |
|--------------------|----------|
| `correctLetters`   | `string` |
| `misplacedLetters` | `string` |
| `excludeLetters`   | `string` |


**Example:**

```json
{
  "correctLetters": "a__le",
  "misplacedLetters": "t",
  "excludeLetters": "xyz"
}
```

---

### Response

**Status Code:** `200 OK`

#### Body

| Name        | Type              |
|-------------|-------------------|
| `messages`  | `array of string` |
| `words`     | `array of string` |


**Example:**

```json
{
  "messages": ["Found 2 Word(s) matching the criteria."],
  "words": ["apple", "angle"]
}
```

---

## 🏷️ Tag

- `Wizdle.Api`
