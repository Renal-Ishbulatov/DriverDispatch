\# DriverDispatch



Подбор водителей на заказ. Карта — прямоугольная сетка N×M, в каждой клетке не более одного водителя. Координаты — целые числа `X`, `Y`, где `0 <= X < N`, `0 <= Y < M`. Каждый водитель имеет уникальный идентификатор.



\## Структура решения



\- `DriverDispatch.Core` — модели, интерфейс и алгоритмы поиска.

\- `DriverDispatch.Console` — демонстрация работы (часть 1).

\- `DriverDispatch.Tests` — NUnit-тесты для всех алгоритмов.

\- `DriverDispatch.Benchmarks` — BenchmarkDotNet для сравнения производительности.



\## Алгоритмы поиска ближайших водителей



1\. \*\*LinearSearchAlgorithm\*\* — вычисляет квадрат расстояния до всех водителей и полностью сортирует список. Сложность `O(n log n)`.

2\. \*\*PartialSortAlgorithm\*\* — держит max-heap размера `k` (через `PriorityQueue` с инвертированным приоритетом), заменяет худшего кандидата, если найден более близкий. Сложность `O(n log k)`.

3\. \*\*GridSearchAlgorithm\*\* — использует сеточную структуру карты: строит индекс «координата → водитель» и обходит кольца вокруг заказа, пока не наберёт `k` водителей.



\## Запуск



```bash

\# Демонстрация

dotnet run --project DriverDispatch.Console



\# Тесты

dotnet test



\# Бенчмарки (обязательно в Release)

dotnet run -c Release --project DriverDispatch.Benchmarks

```



\## Демонстрация работы



!\[Консольное демо](screenshots/02-console-demo.png)



\## Тесты



!\[Результаты тестов](screenshots/03-tests.png)



\## Результаты бенчмарков



Сравнение производительности трёх алгоритмов (BenchmarkDotNet, .NET 6, Release).



!\[Результаты BenchmarkDotNet](screenshots/04-benchmarks.png)

