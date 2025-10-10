# Hello World Component

A simple example component that demonstrates the basic patterns and structure for building Apps SDK components in this repository.

## What This Component Demonstrates

This component serves as a learning resource that shows:

1. **Basic React Component Structure**: How to create a functional React component that can be used with the Apps SDK
2. **Tailwind CSS Styling**: Using the repository's Tailwind configuration for consistent styling
3. **Lucide React Icons**: Integrating icon libraries into your components
4. **Component Export Pattern**: The correct way to export and mount components for both development and production builds

## Files

- `hello-world.jsx` - The main component implementation
- `index.jsx` - Entry point that mounts the component to the DOM
- `README.md` - This file

## Running Locally

### Development Mode

```bash
pnpm run dev
```

Then navigate to http://localhost:4444/hello-world.html

### Production Build

```bash
pnpm run build
```

This will generate:
- `assets/hello-world-{hash}.html` - Standalone HTML file with inlined CSS and JS
- `assets/hello-world-{hash}.css` - Extracted CSS
- `assets/hello-world-{hash}.js` - Compiled JavaScript

## Key Patterns to Learn

### 1. Component Structure

The main component is a simple functional React component:

```jsx
function App() {
  return (
    <div className="...">
      {/* Component content */}
    </div>
  );
}

export default App;
```

### 2. Tailwind CSS Classes

The component uses Tailwind utility classes for styling:
- Layout: `flex`, `grid`, `space-y-*`
- Spacing: `p-*`, `m-*`, `gap-*`
- Colors: `bg-blue-500`, `text-black/70`
- Borders: `rounded-xl`, `border-black/10`

### 3. Entry Point Pattern

The `index.jsx` file follows the repository's standard pattern:

```jsx
import { createRoot } from "react-dom/client";
import App from "./hello-world";

createRoot(document.getElementById("hello-world-root")).render(<App />);

export { App };
export default App;
```

Note: The root element ID must match the component name (`hello-world-root`).

### 4. Build Configuration

The component is automatically picked up by `build-all.mts` when:
- It has an `index.jsx` or `index.tsx` file in `src/{component-name}/`
- The component name is added to the `targets` array in `build-all.mts`

## Next Steps

After understanding this example:

1. Explore other components in `src/` for more advanced patterns
2. Create your own component by copying this structure
3. Customize the styling and functionality for your use case
4. Build and test your component locally
5. Deploy your component with your MCP server

## Related Documentation

- [Apps SDK Documentation](https://platform.openai.com/docs/guides/apps-sdk)
- [React Documentation](https://react.dev/)
- [Tailwind CSS Documentation](https://tailwindcss.com/)
- [Lucide React Icons](https://lucide.dev/)
