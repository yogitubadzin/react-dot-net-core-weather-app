import LowerTemperature from "./components/LowestTemperature";
import HighestWindSpeed from "./components/HighestWindSpeed";
import { Home } from "./components/Home";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/lowest-temperatures',
    element: <LowerTemperature />
  },
  {
    path: '/highest-wind-speeds',
    element: <HighestWindSpeed />
  }
];

export default AppRoutes;
