import React, { useState, useEffect } from 'react';
import { Line } from 'react-chartjs-2';
import {
  Chart,
  LinearScale,
  PointElement,
  Legend,
  TimeScale,
  LineController,
  CategoryScale,
  BarElement,
  LineElement,
  Title,
  Tooltip
} from 'chart.js';
import { pl } from 'date-fns/locale';
import 'chartjs-adapter-moment';

const HighestWindSpeed = () => {
  const [weatherData, setWeatherData] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchData = async () => {
    try {
      const response = await fetch('cities/weathers/highest-wind-speeds');
      const data = await response.json();
      setWeatherData(data);
      setLoading(false);
    } catch (error) {
      console.error('Error fetching weather data:', error);
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  if (loading) {
    return <p>Loading...</p>;
  }

  Chart.register(
    LineController,
    CategoryScale,
    LinearScale,
    BarElement,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend,
    TimeScale
  );

  const chartLabels = weatherData.map(item => new Date(item.lastUpdate));

  const chartData = {
    labels: chartLabels,
    datasets: [
      {
        label: 'Wind Speed',
        fill: true,
        lineTension: 0.1,
        backgroundColor: 'rgba(75,192,192,0.4)',
        borderColor: 'rgba(75,192,192,1)',
        data: weatherData.map(item => item.highestWind),
      },
    ],
  };

  const options = {
    scales: {
      x: {
        type: 'time',
        time: {
          unit: 'day',
        },
        title: {
          display: true,
          text: 'Last Update',
        },
        adapters: {
          date: {
            locale: pl,
          },
        },
      },
      y: {
        title: {
          display: true,
          text: 'Wind Speed',
        },
      },
    },
    plugins: {
      legend: {
        display: true,
        position: 'top',
      },
      tooltip: {
        callbacks: {
          title: (tooltipItems, data) => {
            if (tooltipItems && tooltipItems.length > 0) {
              const dataIndex  = tooltipItems[0].dataIndex ;
              const city = weatherData[dataIndex];
              return `${city.cityName} | ${city.countryName} | ${new Date(city.lastUpdate).toLocaleString()}`;
            }
            return '';
          },
          label: (tooltipItem) => `Highest Wind: ${tooltipItem.formattedValue}°C`,
        },
      },
    },
  };

  return <Line data={chartData} options={options} />;
};

export default HighestWindSpeed;
