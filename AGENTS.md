# AGENTS.md

## Zielbild
Dieses Repository folgt den folgenden Architektur- und Qualitätsprinzipien:

1. **Clean Coding**
2. **CQRS (Command Query Responsibility Segregation)**
3. **Mediator-Architektur**

Diese Regeln gelten für den gesamten Projektbaum, sofern in tiefer liegenden `AGENTS.md`-Dateien nichts Spezifischeres definiert ist.

---

## 1) Clean Coding (verbindlich)

- Schreibe **kleine, fokussierte Klassen und Methoden** mit genau einer Verantwortung.
- Nutze **sprechende Namen** (Klassen, Methoden, Variablen), keine kryptischen Abkürzungen.
- Halte Methoden kurz; extrahiere Logik in eigene Methoden/Services statt "God Methods".
- Vermeide Duplikate (**DRY**), aber abstrahiere nur bei echtem Mehrwert.
- Bevorzuge **Guard Clauses** und klare Kontrollflüsse statt tiefer Verschachtelung.
- Behandle Fehler explizit; keine stummen Fehler oder versteckte Seiteneffekte.
- Schreibe testbaren Code: Abhängigkeiten über Interfaces abstrahieren und injizieren.
- Entferne toten Code, auskommentierte Altlogik und nicht genutzte Abhängigkeiten.

---

## 2) CQRS-Regeln (verbindlich)

- **Commands** verändern Zustand und geben keine fachlichen Read-Model-Daten zurück.
- **Queries** lesen Daten und verändern keinen Zustand.
- Für jeden Use Case gilt: entweder Command oder Query, nicht beides vermischen.
- Separate Modelle pro Anwendungsfall (Input/Output), keine unnötig geteilten DTOs.
- Validierung erfolgt use-case-nah (z. B. im Command/Query-Handler oder dedizierter Pipeline).
- Persistenzzugriffe bleiben im jeweiligen Anwendungsfall klar nachvollziehbar.

### Namenskonventionen
- Commands enden auf `Command` (z. B. `CreateOrderCommand`).
- Queries enden auf `Query` (z. B. `GetOrderByIdQuery`).
- Handler enden auf `Handler` (z. B. `CreateOrderCommandHandler`).

---

## 3) Mediator-Architektur (verbindlich)

- Controller/Endpoints enthalten keine Businesslogik, sondern delegieren an den Mediator.
- Jeder Handler implementiert genau einen klaren Anwendungsfall.
- Cross-Cutting Concerns (Logging, Validation, Tracing, Transaktionen) über Pipeline/Behavior lösen.
- Handler dürfen voneinander unabhängig bleiben; direkte Handler-zu-Handler-Aufrufe vermeiden.
- Domainlogik bleibt in Domain/Application und nicht in Transport-/UI-Schichten.

---

## Projektstruktur (Soll)

- `Presentation/`:
  - Controller/HTTP-Modelle, Request-Mapping, Response-Mapping
  - Keine Businesslogik
- `Shared/Application/`:
  - Commands, Queries, Handler, Interfaces, Pipelines
- `Shared/Domain/`:
  - Entitäten, Value Objects, Domain Services, Domänenregeln
- `Shared/Infrastructure/`:
  - Datenzugriff, externe Integrationen, technische Implementierungen

---

## Definition of Done (DoD)

Änderungen gelten nur dann als fertig, wenn:

- Architekturprinzipien oben eingehalten sind.
- Keine Vermischung von Command- und Query-Verantwortung vorliegt.
- Neue Use Cases über Mediator/Handler verdrahtet sind.
- Code verständlich, konsistent benannt und wartbar ist.
- Relevante Tests/Checks erfolgreich laufen.
