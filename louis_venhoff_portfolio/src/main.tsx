import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { ChakraProvider, defaultSystem } from '@chakra-ui/react';
import { ApolloProvider } from '@apollo/client';
import useGithubApi from './hooks/useGithubApi.ts';

const graphQLClient = useGithubApi();


createRoot(document.getElementById('root')!).render(    
    <ChakraProvider value={defaultSystem}>
        <ApolloProvider client={graphQLClient}>
            <App />
        </ApolloProvider>
    </ChakraProvider>
);

