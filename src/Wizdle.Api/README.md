# 🧩 Wizdle.Api

A simple API to help solve word puzzles by narrowing down possible matches based on input clues.

---

## 🎯 `POST /`

**Summary**: Processes a Wizdle request in an attempt to solve the possible words.

### Request

**Content-Type:** `application/json`

#### Body Parameters

| Name               | Type     |
| ------------------ | -------- |
| `correctLetters`   | `string` |
| `misplacedLetters` | `string` |
| `excludeLetters`   | `string` |

**Example:**

```json
{
    "correctLetters": "a...e",
    "misplacedLetters": "t....",
    "excludeLetters": "xyz"
}
```

---

### Response

**Status Code:** `200 OK`

#### Body

| Name       | Type              |
| ---------- | ----------------- |
| `messages` | `array of string` |
| `words`    | `array of string` |

**Example:**

```json
{
    "messages": ["Found 2 Word(s) matching the criteria."],
    "words": ["apple", "angle"]
}
```

---

## 🚦 Rate Limiting

Requests are rate limited per IP address using a fixed window.

| Setting | Default |
| --- | --- |
| Requests per window | `60` |
| Window duration | `60` seconds |
| Rejection status | `429 Too Many Requests` |

Limits are configurable via `appsettings.json`:

```json
"RateLimiting": {
    "PermitLimit": 60,
    "WindowSeconds": 60
}
```

## 📖 API Reference

An interactive API reference powered by [Scalar](https://scalar.com/) is available at `/scalar/v1` when the API is running.

