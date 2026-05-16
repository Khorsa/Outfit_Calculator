GNU GPL v3 license — you can do anything you want, but you must give me credit and distribute your changes under the same terms.

Calculator is a WPF-based desktop calculator application built as a module for the OutfitTool ecosystem. The application provides standard arithmetic operations with additional scientific functions including square, square root, reciprocal (1/x), and natural logarithm (Ln).

Key features:

- **Expression evaluation**: Supports standard arithmetic operations (+, -, ×, /) with proper operator precedence
- **Scientific functions**: Square (x²), square root (√x), reciprocal (1/x), and natural logarithm (Ln)
- **History management**: Stores up to 10 recent calculations with clickable recall from status bar menu
- **Customizable rounding**: Configurable decimal precision from 0 to 10 digits
- **Clipboard integration**: Copy, cut, and paste operations with optional automatic result copying
- **Responsive UI**: Font size scales with window dimensions for improved readability
- **Persistent settings**: Window position, size, and preferences saved between sessions

The application is implemented as a module conforming to the OutfitTool API1 specification, with ModuleController handling lifecycle management and ToggleCommand providing show/hide functionality. The calculator engine (CalculatorMachine) processes expressions by normalizing input and using DataTable.Compute for evaluation, with unary operations processed separately. Settings are managed through CalculatorSettings and persisted via the API's SettingsManager.

---

Лицензия GNU GPL v3 — вы можете делать с этим что угодно, но должны указать авторство и распространять ваши изменения на тех же условиях.

Калькулятор — это настольное приложение на базе WPF, созданное как модуль для экосистемы OutfitTool. Приложение предоставляет стандартные арифметические операции с дополнительными научными функциями, включая возведение в квадрат, извлечение квадратного корня, обратную величину (1/x) и натуральный логарифм (Ln).

Ключевые особенности:

- **Вычисление выражений**: Поддержка стандартных арифметических операций (+, -, ×, /) с правильным приоритетом операторов
- **Научные функции**: Возведение в квадрат (x²), извлечение квадратного корня (√x), обратная величина (1/x) и натуральный логарифм (Ln)
- **Управление историей**: Хранение до 10 последних вычислений с возможностью кликабельного вызова из меню в строке состояния
- **Настройка округления**: Настройка точности десятичных знаков от 0 до 10 цифр
- **Интеграция с буфером обмена**: Операции копирования, вырезания и вставки с опциональным автоматическим копированием результата
- **Адаптивный интерфейс**: Размер шрифта масштабируется в зависимости от размеров окна для улучшения читаемости
- **Постоянные настройки**: Положение окна, размер и предпочтения сохраняются между сеансами

Приложение реализовано как модуль, соответствующий спецификации OutfitTool API1, где ModuleController управляет жизненным циклом, а ToggleCommand обеспечивает функциональность показа/скрытия. Движок калькулятора (CalculatorMachine) обрабатывает выражения путем нормализации ввода и использования DataTable.Compute для вычисления, при этом унарные операции обрабатываются отдельно. Настройки управляются через CalculatorSettings и сохраняются через SettingsManager API.