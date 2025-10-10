import { createRoot } from "react-dom/client";
import App from "./hello-world";

createRoot(document.getElementById("hello-world-root")).render(<App />);

export { App };
export default App;
