# Copilot Instructions

## Architecture
- Follow SOLID, DRY, KISS, and YAGNI principles.
- Prefer composition over inheritance.

## Development Preferences
- Target .NET 10 and the latest ASP.NET Core.
- Optimize commands and file paths for Ubuntu Linux or Windows.
- Use modern C# 14 features when they improve readability.
- Prefer async/await over synchronous APIs when available.
- Prefer dependency injection over manual service instantiation.
- Prefer early returns over deep nesting.
- Respect nullable reference types.
- Use descriptive and meaningful names.
- Prefer LINQ when it improves readability.
- Handle errors explicitly. Do not swallow exceptions.
- Never hardcode secrets, passwords, tokens, or connection strings.

## Code Generation Rules
- Use existing APIs only. Do not invent methods or libraries.
- Keep solutions simple and avoid unnecessary abstractions.
- Preserve existing user comments.
- Write new code comments in English.
- When modifying code, show only changed blocks unless full file output is requested.

## Communication
- Respond in Russian unless explicitly requested otherwise.

## Commit Messages
- Follow the Conventional Commits specification.
- Output exactly one line.
- Output only the commit message text.
- Do not include markdown, code blocks, quotes, or explanations.
- Use English only.
- Use imperative mood.
- Use lowercase for type and scope.
- Scope should represent the affected module, component, or folder.
- Do not end the description with a period.

### Format
- type(scope): description

### Allowed Types
- feat
- fix
- docs
- style
- refactor
- test
- chore

### Examples
- feat(auth): add login validation
- fix(database): resolve migration timeout
- refactor(tasks): simplify command parser
- docs(readme): update installation guide

## Test Generation
- Generate comprehensive test coverage.
- Include happy path, negative cases, and edge cases.
- Test both valid and invalid inputs.
- Use diverse test data instead of repeating similar values.
- Cover boundary values and off-by-one scenarios.
- Verify exception behavior when applicable.
- Verify error messages when applicable.
- Test null, empty, whitespace, and default values where relevant.
- Test minimum and maximum allowed values where relevant.
- Prefer parameterized tests for multiple input variations.
- Keep tests independent and deterministic.
- Avoid duplicated test cases.
- Use clear test names that describe the scenario and expected result.

### Coverage Checklist
- Happy path
- Invalid input
- Boundary values
- Null values
- Empty values
- Whitespace values
- Duplicate values
- Case sensitivity
- Special characters
- Large inputs
- Exception scenarios
- State changes
- Return values